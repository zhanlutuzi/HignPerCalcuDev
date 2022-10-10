#include <windows.h>
#include <stdio.h>
#include <time.h>
#include <omp.h>

const int numThread = 4;
const int n = 1000;
double A1[n][n], A2[n][n];

// 串行程序
void serialLU();

// 并行程序
void multiLU();

double H1(int r, int i)
{
	double re = 0.0;
	for (int k = 0; k < r; k++)
	{
		re += A1[r][k] * A1[k][i];
	}
	return re;
}

double H2(int r, int i)
{
	double re = 0.0;
	for (int k = 0; k < r; k++)
	{
		re += A2[r][k] * A2[k][i];
	}
	return re;
}

// 初始化矩阵
void InitMatrix(double B1[n][n], double B2[n][n])
{
	for (int a = 0; a < n; a++)
	{
		for (int j = 0; j < n; j++)
		{
			B1[a][j] = rand() % 5 + 1.0;
			B2[a][j] = B1[a][j];
		}
	}
}

void serialLU()
{
	double sum1 = 0;
	for (int i = 0; i < n; i++)
	{
		A1[0][i] = A1[0][i];
		A1[i][0] = A1[i][0] / A1[0][0];
	}
	for (int r = 1; r < n; r++)
	{
		A1[r][r] = A1[r][r] - H1(r, r);
		for (int i = r + 1; i < n; i++)
		{
			sum1 = H1(r, i);
			A1[r][i] = A1[r][i] - sum1;
			A1[i][r] = (A1[i][r] - sum1) / A1[r][r];
		}
	}
}

void multiLU()
{
	double sum2;
	for (int i = 0; i < n; i++)
	{
		A2[0][i] = A2[0][i];
		A2[i][0] = A2[i][0] / A2[0][0];
	}
	for (int r = 1; r < n; r++)
	{
		A2[r][r] = A2[r][r] - H1(r, r);
		#pragma omp parallel for private(sum2) schedule(guided)
		for (int i = r + 1; i < n; i++)
		{
			sum2 = H2(r, i);
			A2[r][i] = A2[r][i] - sum2;
			A2[i][r] = (A2[i][r] - sum2) / A2[r][r];
		}
	}
}

int main()
{
	// 计时
	double start, end, diffTime1, diffTime2;
	InitMatrix(A1, A2);
	// 串行程序
	start = clock();
	serialLU();
	end = clock();
	diffTime1 = (end - start) / 1000;
	printf("serial time %.3f seconds\n", diffTime1);

	// 多线程程序
	omp_set_num_threads(numThread);
	start = clock();
	multiLU();
	end = clock();
	diffTime2 = (end - start) / 1000;
	printf("parallel time %.3f seconds \n", diffTime2);
	printf("speedup = %.3f", diffTime1 / diffTime2);
	return 0;
}


