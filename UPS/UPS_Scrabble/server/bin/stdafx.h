//
// Created by pivov on 23-Dec-16.
//
#pragma once

#ifndef UPS_SCRABBLE_SERVER_STDAFX_H
#define UPS_SCRABBLE_SERVER_STDAFX_H

//IO
#include <iostream>
#include <stdio.h>

//std dlls
#include <string.h>
#include <list>
#include <vector>

//BSD sockets
#include <sys/socket.h>
#include <arpa/inet.h> //inet_addr
#include <unistd.h>

//threads
#include <thread>
#include <pthread.h>


using namespace std;

//#define nullptr ( (void *) 0)
#define msg_length 1024

#endif //UPS_SCRABBLE_SERVER_STDAFX_H
