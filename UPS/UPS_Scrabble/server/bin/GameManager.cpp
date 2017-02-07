//
// Created by pivov on 01-Jan-17.
//

#include "GameManager.h"

int GameManager::game_count = 0;
int GameManager::player_count[3] = {0, 0, 0};

vector<Game*> GameManager::GameList;
vector<Player*> GameManager::PlayerList[3] = {vector<Player*>(), vector<Player*>(), vector<Player*>()};


Player* GameManager::PlayerConnect(string nick, char *ip, int socket, int n)
{
    Player *pl;

    pl = new Player(nick, n, ip, socket, PlayerList[n-2].size());
    PlayerList[n-2].push_back(pl);
    player_count[n-2]++;

    if((player_count[n-2] % n) == 0){
        GameManager::StartGame(n);
    }
    return pl;
}

void GameManager::PlayerConnect(Player *pl)
{
    int n = pl->n;
    pl->id = PlayerList[n-2].size();

    PlayerList[n-2].push_back(pl);
    player_count[n-2]++;

    if((player_count[n-2] % n) == 0){
        GameManager::StartGame(n);
    }
}

void GameManager::PlayerReconnect(Player *pl)
{
    cout << "Player " << pl->nick << " reconnected!" << endl;

    pl->ClonePlayer(GameManager::GetPlayer(pl->nick, pl->n));
    pl->connected = 0;

    for(int i = PlayerList[pl->n -2].size() - 1; i > -1; i--){
        if(pl->nick == PlayerList[pl->n -2][i]->nick && PlayerList[pl->n -2][i]->connected < 2){
            PlayerList[pl->n -2][i] = pl;
            break;
        }
    }

    for(int i = 0; i < GameList.size(); i++){
        if(GameList[i]->id == pl->GameID){
            GameList[i]->Reconnect(pl);
            break;
        }
    }
}

void GameManager::PlayerDisconnect(Player *pl)
{
    pl->connected = 1;

    if(pl->GameID == -1){
        cout << "Player " << pl->nick << " destroyed!" << endl;

        if(pl->connected == 1) player_count[pl->n-2]--;

        pl->connected = 2;
        //delete(pl);
        return;
    }

    for(int i = 0; i < GameList.size(); i++){
        if(GameList[i]->id == pl->GameID){
            GameList[i]->Disconnect(pl->id);
            if(GameList[i]->PlayerCount == GameList[i]->PlayerDisconnected){
                GameManager::DestroyGame(GameList[i]);
            }
            break;
        }
    }
}

void GameManager::ResolveTurn(string msg)
{
    size_t i = msg.find(':', 0);

    int id = stoi(msg.substr(0, i));
    msg = msg.substr(i + 1);

    for(int i = 0; i < GameList.size(); i++){
        if(GameList[i]->id == id){
            GameList[i]->RecvTurn(msg);
            break;
        }
    }
}

Player* GameManager::GetPlayer(string nick, int n)
{
    for(int i = PlayerList[n-2].size() - 1; i > -1; i--){
        if(nick == PlayerList[n-2][i]->nick && PlayerList[n-2][i]->connected < 2){
            return PlayerList[n-2][i];
        }
    }

    return nullptr;
}

int GameManager::CheckNick(string nick, int n)
{
    for(int i = PlayerList[n-2].size() - 1; i > -1; i--){
        if(nick == PlayerList[n-2][i]->nick) {
            if(PlayerList[n-2][i]->connected == 0) return 2; //pripojen hrajici, nick neni k dispozici
            if(PlayerList[n-2][i]->connected == 1) return 1; //znovu pripojeni do hry
        }
    }

    return 0 ; // nick je volny
}

void GameManager::StartGame(int n)
{
    Game *game;

    switch (n){
        case 2:
            game = new Game(game_count, PlayerList[n-2].at(PlayerList[n-2].size() - 2), PlayerList[n-2].at(PlayerList[n-2].size() - 1));
            break;
        case 3:
            game = new Game(game_count, PlayerList[n-2].at(PlayerList[n-2].size() - 3), PlayerList[n-2].at(PlayerList[n-2].size() - 2), PlayerList[n-2].at(PlayerList[n-2].size() - 1));
            break;
        case 4:
            game = new Game(game_count, PlayerList[n-2].at(PlayerList[n-2].size() - 4), PlayerList[n-2].at(PlayerList[n-2].size() - 3), PlayerList[n-2].at(PlayerList[n-2].size() - 2), PlayerList[n-2].at(PlayerList[n-2].size() - 1));
            break;
        default:
            game = nullptr;
            break;
    }

    GameList.push_back(game);
    game_count++;
}

void GameManager::DestroyGame(Game *g)
{
    cout << "Game " << g->id << " destroyed!" << endl;

    for(int i = 0; i < g->PlayerCount; i++){
        cout << "Player " << g->Players[i]->nick << " destroyed!" << endl;
        g->Players[i]->connected = 2;
        //delete(g->Players[i]);
    }

    //delete(g);
}

void GameManager::Remove(Player *pl)
{
    int n = pl->n;

    for(int i = 0; i < PlayerList[n-2].size(); i++){
        if(PlayerList[n-2][i]->id == pl->id){
            PlayerList[n-2].erase(PlayerList[n-2].begin() + i);
            delete(pl);
            return;
        }
    }
}