#include <windows.h>
#include <gdiplus.h>
#include <ctime>
#include <string>

#pragma comment (lib, "Gdiplus.lib")

using namespace Gdiplus;

LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE, LPSTR, int nCmdShow)
{
    GdiplusStartupInput gdiplusStartupInput;
    ULONG_PTR gdiplusToken;
    GdiplusStartup(&gdiplusToken, &gdiplusStartupInput, NULL);

    WNDCLASSA wc = {};
    wc.lpfnWndProc = WndProc;
    wc.hInstance = hInstance;
    wc.lpszClassName = "GDIP_DEMO";
    RegisterClassA(&wc);

    HWND hwnd = CreateWindowA(
        "GDIP_DEMO",
        "Privit",
        WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, CW_USEDEFAULT,
        700, 500,
        NULL, NULL, hInstance, NULL
    );

    ShowWindow(hwnd, nCmdShow);
    UpdateWindow(hwnd);

    srand(time(NULL));

    MSG msg;
    while (GetMessage(&msg, NULL, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    GdiplusShutdown(gdiplusToken);
    return 0;
}

Color randomColor()
{
    return Color(
        255,
        rand() % 256,
        rand() % 256,
        rand() % 256
    );
}

void DrawColumn(Graphics& g, int x, int yStart, bool growDown)
{
    int sizeStart = 14;
    int step = 5;

    const WCHAR text[] = L"Привіт!";

    for (int i = 0; i < 6; i++)
    {
        int fontSize = growDown ? (sizeStart + i * step)
                                : (sizeStart + (5 - i) * step);

        Font font(L"Arial", (REAL)fontSize);
        SolidBrush brush(randomColor());

        g.DrawString(
            text,
            -1,
            &font,
            PointF((REAL)x, (REAL)(yStart + i * 40)),
            &brush
        );
    }
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
    switch (msg)
    {
    case WM_PAINT:
    {
        PAINTSTRUCT ps;
        HDC hdc = BeginPaint(hwnd, &ps);

        Graphics g(hdc);
        g.SetSmoothingMode(SmoothingModeAntiAlias);

        DrawColumn(g, 50, 50, true);   // лівий
        DrawColumn(g, 280, 50, true);  // правий
        DrawColumn(g, 165, 50, false); // середній

        EndPaint(hwnd, &ps);
    }
    break;

    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    }

    return DefWindowProc(hwnd, msg, wParam, lParam);
}
