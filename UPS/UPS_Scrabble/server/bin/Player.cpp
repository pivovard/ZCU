//
// Created by pivov on 25-Dec-16.
//

#include "Player.h"
#include "GameManager.h"

Player::Player(char *ip, int socket)
{
    this->nick = "";
    this->n = -1;
    this->ip = ip;
    this->socket = socket;
    this->id = -1;

    this->message_in = new char[msg_length];
    this->message_out = new char[msg_length];
}

Player::Player(string nick, int n, char *ip, int socket, int id)
{
    this->nick = nick;
    this->n = n;
    this->ip = ip;
    this->socket = socket;
    this->id = id;

    this->message_in = new char[msg_length];
    this->message_out = new char[msg_length];
}

void Player::SendToPlayer(string msg)
{
    cout << "Send to " << this->id << this->nick << ": " << msg << endl;

    ssize_t size = 0;
    while(size < msg.length()){
        size = send(this->socket , msg.c_str() , msg_length, 0);
    }
}

void Player::ClonePlayer(Player *pl)
{
    this->id = pl->id;
    this->GameID = pl->GameID;
    this->score = pl->score;
}