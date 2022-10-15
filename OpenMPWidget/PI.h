#pragma once

#include <QWidget>
#include <QString>
#include <QDateTime>
#include "ui_PI.h"
#include <omp.h>

class PI : public QWidget
{
	Q_OBJECT

public:
	PI(QWidget* parent = nullptr);
	~PI();

private slots:
	void startExcute();

private:
	Ui::PIClass ui;
	// 串行程序
	double serialPI();
	// 并行程序
	double multiPI();
	int numSteps = 2000000;
	int numThread = 4;

};
