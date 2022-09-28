#include <omp.h>
#include <time.h>
#include <stdio.h>

static long num_steps = 100000;
double step;
int main()
{
	double starttime = clock();
	int i;
	double x, pi, sum = 0.0;
	step = 1.0 / (double)num_steps;
#pragma omp parallel
	{
		for (i = 0; i < num_steps; i++)
		{
			x = (i + 0.5) * step;
			sum = sum + 4.0 / (1.0 + x * x);
		}
	}
	pi = step * sum;
	double endtime = clock();
	double timecost = endtime - starttime;
	printf("parallel totally use time is (%.3f) s \n", timecost);
	printf("PI is (%.3f)\n", pi);

	double starttime1 = clock();
	for (i = 0; i < num_steps; i++)
	{
		x = (i + 0.5) * step;
		sum = sum + 4.0 / (1.0 + x * x);
	}
	pi = step * sum;

	double endtime1 = clock();
	double timecost1 = endtime1 - starttime1;
	printf("without parallel totally use time is (%.3f) s \n", timecost1);
	printf("PI is (%.3f) \n", pi);

}