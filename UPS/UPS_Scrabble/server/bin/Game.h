//
// Created by pivov on 25-Dec-16.
//

#ifndef UPS_SCRABBLE_SERVER_GAME_H
#define UPS_SCRABBLE_SERVER_GAME_H

#include "stdafx.h"

#include "Player.h"


class Game
{

public:
    int id;
    char matrix[15][15];

    int PlayerCount;
    int PlayerNext;
    int PlayerOnTurn;
    int PlayerDisconnected;

    vector<Player*> Players;

    Game(int id, Player *pl1, Player *pl2);
    Game(int id, Player *pl1, Player *pl2, Player *pl3);
    Game(int id, Player *pl1, Player *pl2, Player *pl3, Player *pl4);

    void RecvTurn(string msg);
    void Reconnect(Player *pl);
    void Disconnect(int id);

private:

    void Init();

    void NextTurn();
    void SendTurn(string msg);
    int CheckScore(string msg);

    static int multiplier[];
};

#endif //UPS_SCRABBLE_SERVER_GAME_H
