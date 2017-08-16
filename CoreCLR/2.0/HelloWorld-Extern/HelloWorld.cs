using System;
using System.Runtime.InteropServices;

public class Program
{
	[DllImport("libc")]
	private static extern int printf(string format);
	[DllImport("libc")]
	private static extern int getpid();

	public static void Main(string[] args)
	{
		printf("Hello World from process #" + getpid().ToString() + "\n");
	}
}
