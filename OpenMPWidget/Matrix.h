#pragma once

#include <QWidget>
#include "ui_Matrix.h"
#include <QDateTime>
#include <omp.h>

class Matrix : public QWidget
{
	Q_OBJECT

public:
	Matrix(QWidget* parent = nullptr);
	~Matrix();

private:
	Ui::MatrixClass ui;
	int numThread = 4;
	int n = 1000;
	double A1[1000][1000];
	double A2[1000][1000];

	// 串行程序
	void serialLU();
	// 并行程序
	void multiLU();
	double H1(int r, int i);
	double H2(int r, int i);
	// 初始化矩阵
	void InitMatrix(double(*B1)[1000], double(*B2)[1000]);

private slots:
	void startExcute();
};
