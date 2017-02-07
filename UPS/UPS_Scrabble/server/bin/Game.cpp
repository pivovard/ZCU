//
// Created by pivov on 25-Dec-16.
//

#include "Game.h"

Game::Game(int id, Player *pl1, Player *pl2)
{
    this->id = id;
    this->PlayerCount = 2;

    this->Players.push_back(pl1);
    this->Players.push_back(pl2);

    this->Init();
}

Game::Game(int id, Player *pl1, Player *pl2, Player *pl3)
{
    this->id = id;
    this->PlayerCount = 3;

    this->Players.push_back(pl1);
    this->Players.push_back(pl2);
    this->Players.push_back(pl3);

    this->Init();
}

Game::Game(int id, Player *pl1, Player *pl2, Player *pl3, Player *pl4)
{
    this->id = id;
    this->PlayerCount = 4;

    this->Players.push_back(pl1);
    this->Players.push_back(pl2);
    this->Players.push_back(pl3);
    this->Players.push_back(pl4);

    this->Init();
}

void Game::Init()
{

    this->PlayerNext = -1;
    this->PlayerOnTurn = 0;
    this->PlayerDisconnected = 0;

    for(int i = 0; i < 15; i++){
        for(int j = 0; j < 15; j++){
            this->matrix[i][j] = '\0';
        }
    }

    string msg = "GAME:" + to_string(id) + ":";
    for(int i = 0; i < PlayerCount; i++){
        Players[i]->GameID = id;

        msg += to_string(Players[i]->id);
        msg += ",";
        msg += Players[i]->nick;
        msg += ";";
    }
    msg = msg.substr(0, msg.length() - 1);
    msg += "\n";

    for(int i = 0; i < PlayerCount; i++){
        Players[i]->SendToPlayer(msg);
    }

    cout << "Game " << id << " created." << endl;

    this->NextTurn();
}



void Game::NextTurn()
{
    if((PlayerCount - PlayerDisconnected) < 2) return; //pokud je pocet mensi jak 2, nelze hrat
    if(PlayerOnTurn == 1) return; //pokud uz je nejaky hrac na tahu

    PlayerNext++;
    if(PlayerNext == PlayerCount) PlayerNext = 0;

    //pokud je hrac pripojen, posle se mu turn
    if(Players[PlayerNext]->connected == 0){
        Players[PlayerNext]->SendToPlayer("TURN\n");
        this->PlayerOnTurn = 1;
    }
    else {
        this->NextTurn();
    }
}

void Game::RecvTurn(string msg)
{
    this->PlayerOnTurn = 0;

    size_t i = msg.find(';');
    //NextTurn, pokud nejsou zadne tahy
    if(i == std::string::npos){
        this->NextTurn();
        return;
    }

    //preposlani tahu ostatnim hracum
    this->SendTurn(msg);

    //score
    Players[PlayerNext]->score = stoi(msg.substr(0, i));

    int x;
    int y;
    char c;
    size_t  j;
    size_t  k;
    msg = msg.substr(i + 1);

    //doubles of numbers
    while((i = msg.find(';')) != string::npos){
        j = msg.find(',');
        x = stoi(msg.substr(0, j));
        msg = msg.substr(j + 1);

        j = msg.find(',');
        y = stoi(msg.substr(0, j));
        msg = msg.substr(j + 1);

        c = msg.at(0);
        matrix[x][y] = c;

        msg = msg.substr(2); //msg = msg.substr(i + 1);
    }

    j = msg.find(',');
    x = stoi(msg.substr(0, j));
    msg = msg.substr(j + 1);

    j = msg.find(',');
    y = stoi(msg.substr(0, j));
    msg = msg.substr(j + 1);

    c = msg.at(0);
    matrix[x][y] = c;

    this->NextTurn();
}

void Game::SendTurn(string msg)
{
    msg = "TURNP:" + to_string(Players[PlayerNext]->id) + ":" + msg + "\n";

    for(int i = 0; i < PlayerCount; i ++){
        if(i == PlayerNext) continue;
        if(Players[i]->connected == 0) Players[i]->SendToPlayer(msg);
    }
}

void Game::Reconnect(Player *pl)
{
    string msg = "GAMER:" + to_string(this->id) + ":";
    for(int i = 0; i < PlayerCount; i++){
        msg += to_string(Players[i]->id);
        msg += ",";
        msg += Players[i]->nick;
        msg += ",";
        msg += to_string(Players[i]->score);
        msg += ";";
    }
    msg = msg.substr(0, msg.length() - 1);
    msg += ":";
    for(int i = 0; i < 15; i++){
        for(int j = 0; j < 15; j++){
            if(matrix[i][j] != '\0'){
                msg += to_string(i) + "," + to_string(j) + "," + matrix[i][j];
                msg += ";";
            }
        }
    }
    msg = msg.substr(0, msg.length() - 1);
    msg += "\n";

    string msg2 = "RECN:" + to_string(pl->id) + "\n";

    for(int i = 0; i < PlayerCount; i ++) {
        if (Players[i]->id == pl->id) {
            Players[i] = pl;
            Players[i]->SendToPlayer(msg);
        }
        else{
            Players[i]->SendToPlayer(msg2);
        }
    }

    this->PlayerDisconnected--;
    //if((PlayerCount - PlayerDisconnected) == 2) this->NextTurn(); // pripojeni druheho hrace, znovu start hry
    if(this->PlayerOnTurn == 0) this->NextTurn(); // pripojeni druheho hrace, znovu start hry
}

void Game::Disconnect(int id)
{
    this->PlayerDisconnected ++;
    string msg = "DISC:" + to_string(id) + "\n";
    for(int i = 0; i < PlayerCount; i ++){
        if(Players[i]->connected == 0) Players[i]->SendToPlayer(msg);
    }

    if(Players[PlayerNext]->id == id){
        this->PlayerOnTurn = 0;
        this->NextTurn(); //pokud byl odpojeny hrac na tahu
    }
}