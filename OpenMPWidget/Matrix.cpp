#include "Matrix.h"

Matrix::Matrix(QWidget* parent)
	: QWidget(parent)
{
	ui.setupUi(this);
	InitMatrix(A1, A2);
	connect(ui.StartBtn, SIGNAL(clicked(bool)), this, SLOT(startExcute()));
}

Matrix::~Matrix()
{}

void Matrix::startExcute()
{
	double serialTime, parallelTime, startTime, endTime, speedup;

	numThread = ui.ThreadnumEd->text().toInt();
	// 串行
	startTime = QDateTime::currentMSecsSinceEpoch();
	serialLU();
	endTime = QDateTime::currentMSecsSinceEpoch();
	serialTime = endTime - startTime;

	// 并行
	startTime = QDateTime::currentMSecsSinceEpoch();
	multiLU();
	endTime = QDateTime::currentMSecsSinceEpoch();
	parallelTime = endTime - startTime;

	speedup = serialTime / parallelTime;

	//显示
	QString strSeriTm = QString::number(serialTime / 1000);
	strSeriTm = strSeriTm.append("秒");
	ui.serialTimeEd->setText(strSeriTm.toUtf8());

	QString strParaTm = QString::number(parallelTime / 1000);
	strParaTm = strParaTm.append("秒");
	ui.paralleltimeEd->setText(strParaTm.toUtf8());

	QString strSpeedup = QString::number(speedup);
	ui.speedupEd->setText(strSpeedup);
}

void Matrix::serialLU()
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

void Matrix::multiLU()
{
	omp_set_num_threads(numThread);
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

double Matrix::H1(int r, int i)
{
	double re = 0.0;
	for (int k = 0; k < r; k++)
	{
		re += A1[r][k] * A1[k][i];
	}
	return re;
}

double Matrix::H2(int r, int i)
{
	double re = 0.0;
	for (int k = 0; k < r; k++)
	{
		re += A2[r][k] * A2[k][i];
	}
	return re;
}

void Matrix::InitMatrix(double(*B1)[1000], double(*B2)[1000])
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
