#include "stdafx.h"
#include "App.h"

int main(int argc, char* argv[]) {
    cout << "Server start!" << endl;

    //set port
    if(argc == 2){
        int p = atoi(argv[1]);
        if(p > 1024 && p < 65536) Network::PORT = p;
        else cout << "Ilegal port value, port se to 1993." << endl;
    }

    App();

    cout << "Server end!" << endl;
    return 0;
}