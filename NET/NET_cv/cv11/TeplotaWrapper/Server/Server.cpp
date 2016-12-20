#include <stdio.h>
#include <windows.h>

extern "C"
{
	bool running;
	int temp = 0;

	typedef void(__stdcall *CallBackFunc) (int);

	void __stdcall TestCallBack(int a)
	{
		printf("teplota: %d\n", a);
	}

	CallBackFunc back = TestCallBack;

	__declspec(dllexport) void __stdcall RegisterCallBack(CallBackFunc call_back) {
		back = call_back;
	}

	__declspec(dllexport) void __stdcall Init() {
		printf("Initializing server...\n");
		running = false;
	}

	__declspec(dllexport) void __stdcall Start() {
		if (running) return;
		running = true;
		while (running) {
			printf("Current temperature: %d\n", temp);
			back(temp++);
			Sleep(1000);
		}
	}

	__declspec(dllexport) void __stdcall Stop() {
		running = false;
	}

	__declspec(dllexport) bool __stdcall IsRunning() {
		printf("Server is running: %s\n", running ? "true" : "false");
		return running;
	}
}