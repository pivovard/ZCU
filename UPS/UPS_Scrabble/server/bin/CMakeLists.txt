cmake_minimum_required(VERSION 3.6)
project(UPS_Scrabble_server)

find_package(Threads REQUIRED)

set(CMAKE_CXX_STANDARD 11)

set(SOURCE_FILES main.cpp Network.cpp stdafx.h Network.h Game.cpp Game.h Player.cpp Player.h App.cpp App.h GameManager.cpp GameManager.h)

add_executable(UPS_Scrabble_server ${SOURCE_FILES})

target_link_libraries (UPS_Scrabble_server ${CMAKE_THREAD_LIBS_INIT})