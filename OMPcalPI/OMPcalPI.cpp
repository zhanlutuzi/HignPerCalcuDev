#include <windows.h>
#include <time.h>
#include <omp.h>
#include <stdio.h>

const int numSteps = 2000000;
const int numThread = 4;

// 串行程序
double serialPI();
// 并行程序
double multiPI();

double serialPI()
{
	double pi = 0.0, x, y;
	int count = 0;
	for (int i = 0; i < numSteps; i++)
	{
		x = rand() * 1.0 / RAND_MAX;
		y = rand() * 1.0 / RAND_MAX;
		if ((x * x + y * y - 1.0) <= 1e-6)
			count++;
	}
	pi = count * 4.0 / numSteps;
	return pi;
}

double multiPI()
{
	double pi = 0.0, x, y;
	int count = 0;
	#pragma omp parallel for private(x,y) reduction(+:count)
	for (int i = 0; i < numSteps; i++)
	{
		x = rand() * 1.0 / RAND_MAX;
		y = rand() * 1.0 / RAND_MAX;
		if ((x * x + y * y - 1.0) <= 1e-6)
			count++;
	}
	pi = count * 4.0 / numSteps;
	return pi;
}


int main()
{
	double result;
	// 计时
	double start, end, diffTime1, diffTime2;
	// serial
	start = clock();
	result = serialPI();
	end = clock();
	diffTime1 = (end - start)/1000;
	printf("PI = %12.12f in serial time %.3f seconds\n", result, diffTime1);

	start = clock();
	result = multiPI();
	end = clock();
	diffTime2 = (end - start) / 1000;
	printf("PI = %12.12f in parallel time %.3f seconds\n", result, diffTime2);

	return 0;
}