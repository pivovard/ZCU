//
// Created by pivov on 25-Dec-16.
//

#include "App.h"

App::App()
{
    std::thread thread_command(App::Command);

    Network::Start();
    std::thread thread_listen(Network::Listen);
    thread_listen.detach();

    thread_command.join();
}

void App::Command()
{
    while(true)
    {
        string out;
        cin >> out;
        if(out.compare("exit") == 0)
        {
            cout << "Server shutting down!" << endl;
            Network::Exit();
            break;
        }
    }
}




