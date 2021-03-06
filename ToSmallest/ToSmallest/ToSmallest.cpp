#include "pch.h"
#include <iostream>
#include <vector>
#include <array>
#include <string>

using namespace std;
unsigned int number_of_digits = 0;

int numberofdig(int n) 
{
	do
	{
		++number_of_digits;
		n /= 10;
	} while (n);
	return number_of_digits;
}

std::vector<long long> shufflearray(int* arr, int number_of_digits) 
{
	int concat = 0;
	int lownumber = 0;
	int newposition = 0;
	int position = 0;
	for (int z = number_of_digits - 1; z >= 0; z--) 
{
	for (int x = number_of_digits-1; x >=0; x--)
	{
			concat = arr[x];
			for (int i = number_of_digits - 1; i >= 0; i--)
			{
				if (i != x)
				{
					concat = concat * 10 + arr[i];
				}
			}
			if (lownumber == 0)
			{
				lownumber = concat;
				position = (number_of_digits - 1) - x;
				newposition = 0;
			}
			else if (concat <= lownumber)
			{
				lownumber = concat;
				position = (number_of_digits - 1) - x;
				newposition = 0;
			}
		}
	}
	lownumber;
	
	return { lownumber,position,newposition };
}

std::vector<long long> digtoarray(int number_of_digits,int n)
{
	int *arr = new int[number_of_digits];
	int y = 0;
	for (int i = 0; i < number_of_digits; i++)
	{
		arr[i] = n % 10;
		n = n / 10;
	}
	std::vector<long long> v = shufflearray(arr,number_of_digits);
	return v;
}

std::vector<long long> smallest(long long n)
{
	int num = numberofdig(n);
	std::vector<long long> v =digtoarray(num, n);
	return v;
}

int main()
{
	std::cout << "Starting...\n";
	std::vector<long long> v = smallest(261235);
	std::string str = "{" + std::to_string(v[0]) + "," + std::to_string(v[1]) + "," + std::to_string(v[2]) + "}";
	std::cout << str;
};