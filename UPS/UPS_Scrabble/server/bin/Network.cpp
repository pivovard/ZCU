//
// Created by pivov on 23-Dec-16.
//

#include "Network.h"


int Network::socket_desc , Network::client_sock , Network::c;
struct sockaddr_in Network::server , Network::client;
int Network::PORT = 1993;

void Network::Start()
{
    //Create socket
    socket_desc = socket(AF_INET , SOCK_STREAM , IPPROTO_TCP);
    if (socket_desc == -1)
    {
        printf("Could not create socket");
        return;
    }
    puts("Socket created.");

    //set port release
    int enable = 1;
    if (setsockopt(socket_desc, SOL_SOCKET, SO_REUSEADDR, &enable, sizeof(int)) < 0)
        perror("setsockopt(SO_REUSEADDR) failed");

    //set timeout
    /*struct timeval timeout;
    timeout.tv_sec = 1;
    timeout.tv_usec = 0;

    if (setsockopt (socket_desc, SOL_SOCKET, SO_RCVTIMEO, (char *)&timeout, sizeof(timeout)) < 0)
        perror("setsockopt failed\n");

    if (setsockopt (socket_desc, SOL_SOCKET, SO_SNDTIMEO, (char *)&timeout, sizeof(timeout)) < 0)
        perror("setsockopt failed\n");*/

    //Prepare the sockaddr_in structure
    server.sin_family = AF_INET;
    server.sin_addr.s_addr = INADDR_ANY;
    server.sin_port = htons( Network::PORT );

    //Bind
    if( bind(socket_desc,(struct sockaddr *)&server , sizeof(server)) < 0)
    {
        //print the error message
        perror("Bind failed. Error");
        return;
    }
    puts("Bind done.");

    //Listen
    if( listen(socket_desc , 3) < 0 )
    {
        //print the error message
        perror("Listen failed. Error");
        return;
    }

    //Accept and incoming connection
    puts("Server ready!");
}


void Network::Listen()
{
    ssize_t size;
    char *nick_in = new char[64];
    int i;

    c = sizeof(struct sockaddr_in);
    while( (client_sock = accept(socket_desc, (struct sockaddr *)&client, (socklen_t*)&c)) )
    {
        puts("Connection accepted.");

        char *ip = inet_ntoa(client.sin_addr);
        Player *pl = new Player(ip, client_sock);
        //Player *pl = GameManager::PlayerConnect(nick, ip, client_sock, n);

        std::thread thread_pl(Network::PlayerListen, pl);
        thread_pl.detach();

        std::thread thread_ping(Network::PlayerPing, pl);
        thread_ping.detach();
    }

    if (client_sock < 0)
    {
        perror("Accept failed.");
    }
}

void Network::PlayerListen(Player *pl)
{
    ssize_t size;
    string msg;
    size_t i;

    cout << "Listenning player: " << pl->nick << "   " << pl->ip << endl;
    while( (size = recv(pl->socket , pl->message_in , msg_length , 0)) > 0){

        msg = string(pl->message_in);
        i = msg.find('\n');
        if (i!=std::string::npos) {
            msg = msg.substr(0, i);
        }
        else{
            continue;
        }

        cout << "Recv from " << pl->id << " " << pl->nick << ": " << msg << endl;
        try{
            Resolve(msg , pl);
        } catch(...){
            cout << "Message not resolved." << endl;
            pl->SendToPlayer("ERR:0\n");
        }
    }

    if(size == 0)
    {
        puts("Client disconnected");
    }
    else if(size == -1)
    {
        puts("Client disconnected");
        perror("recv failed");
    }

    try {
        GameManager::PlayerDisconnect(pl); //uz muze byt nullptr pokud END
    }
    catch(...) {};
}

void Network::Resolve(string msg, Player *pl)
{
    size_t i = msg.find(':', 0);
    string type = msg;

    if(i!=std::string::npos){
        type = msg.substr(0, i);
        msg = msg.substr(i + 1);
    }

    if(strcmp(type.c_str(), "TURN") == 0){
        try {
            GameManager::ResolveTurn(msg, pl);
        } catch (...){
            pl->SendToPlayer("TURNERR\n");
        }
    }
    else if(strcmp(type.c_str(), "NICK") == 0){
        i = msg.find(';');
        int n = atoi(msg.substr(i+1).c_str());
        if(n < 2 || n > 4){
            pl->SendToPlayer("NICKERR:CHAR\n");
            close(pl->socket);
            return;
        }

        pl->n = n; // stoi(msg.substr(i+1));
        pl->nick = msg.substr(0, i);

        int res = GameManager::CheckNick(pl->nick, pl->n);
        //existujici nick
        if(res == 2){
            pl->SendToPlayer("NICKERR:USE\n");
            close(pl->socket);
            return;
        }
        //odpojeny klient
        if(res == 1){
            char *m = new char[msg_length];
            pl->SendToPlayer("RETURN\n");
        }
        //volny nick
        if(res == 0){
            pl->SendToPlayer("NICKOK\n");
            GameManager::PlayerConnect(pl);
        }
    }
    else if(strcmp(type.c_str(), "RETURN") == 0){
        GameManager::PlayerReconnect(pl);
    }
    else if(strcmp(type.c_str(), "NEW") == 0){
        Player *p = GameManager::GetPlayer(pl->nick, pl->n);
        p->connected = 2;

        GameManager::PlayerConnect(pl);
    }
    else if(strcmp(type.c_str(), "END") == 0){
        return;
    }
    else if(strcmp(type.c_str(), "PING") == 0){
        pl->ping--;
        pl->SendToPlayer("PING\n");
        return;
    }
    else if(strcmp(type.c_str(), "ERR") == 0){
        cout << "Error: " << msg << endl;
        return;
    }
    else {
        cout << "Message not resolved, client killed!" << endl;
        close(pl->socket);
        GameManager::PlayerDisconnect(pl);
    }
}

void Network::PlayerPing(Player * pl)
{
    while(pl->connected == 0){
        pl->ping++;
        if(pl->ping > 5){
            GameManager::PlayerDisconnect(pl);
            break;
        }

        sleep(5);
    }
}

void Network::Exit()
{
    cout << "Socket closed." << endl;
    close(socket_desc);
}







string Network::CropMsg(char *in, ssize_t size)
{
    string msg = "";

    for(int i = 0; i < size; i++){
        msg += in[i];
    }

    return msg;
}

//nefunguje, je treba spravit
char* Network::CropChar(char *in, ssize_t size)
{
    char *out = new char[size];

    for(int i = 0; i < size; i++){
        if(in[i] == '\n') break;
        out[i] = in[i];
    }

    return out;
}

void Network::SendMessage(int socket, string *message)
{
    write(socket , message , (*message).length());
}

void Network::RecvMessage(int socket, string *message)
{
    recv(socket, message, 1024, 0);
}