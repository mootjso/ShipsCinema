﻿using System;
using System.Threading;

public class LoadingBar
{
    public static void StartLoadingBar()
    {
        int consoleWidth = Console.WindowWidth;
        Console.CursorVisible = false;

        Console.SetCursorPosition(consoleWidth - 80, Console.WindowHeight / 2);

        for (int i = 0; i <= 100; i += 5)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow; //CHANGE COLOR
            Console.Write("\r\t\t\t\t\tLoading:{0,-20} {1,3}%", new string('█', i / 5), i);
            Thread.Sleep(150);
        }
    }
}