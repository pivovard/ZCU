//
// Created by pivov on 25-Dec-16.
//

#ifndef UPS_SCRABBLE_SERVER_PLAYER_H
#define UPS_SCRABBLE_SERVER_PLAYER_H

#include "stdafx.h"

class Player
{
public:
    int id;
    string nick;
    int n;
    char *ip;
    int socket;

    int connected = 0;
    int ping = 0;

    int GameID = -1;
    int score = 0;

    char *message_in;
    char *message_out;

    Player(string nick, int n, char *ip, int socket, int id);
    Player(char *ip, int socket);

    void SendToPlayer(string msg);

    void ClonePlayer(Player *pl);
};

#endif //UPS_SCRABBLE_SERVER_PLAYER_H
