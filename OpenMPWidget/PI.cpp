#include "PI.h"

PI::PI(QWidget* parent)
	: QWidget(parent)
{
	ui.setupUi(this);
	connect(ui.StartBtn, SIGNAL(clicked(bool)), this, SLOT(startExcute()));
}

PI::~PI()
{}

void PI::startExcute()
{
	double serialTime, parallelTime, startTime, endTime, speedup, serialRst, paraRst;

	numThread = ui.ThreadnumEd->text().toInt();
	// 串行
	startTime = QDateTime::currentMSecsSinceEpoch();
	serialRst = serialPI();
	endTime = QDateTime::currentMSecsSinceEpoch();
	serialTime = endTime - startTime;

	// 并行
	startTime = QDateTime::currentMSecsSinceEpoch();
	paraRst = multiPI();
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

	QString strSerirst = QString::number(serialRst);
	QString strPararst = QString::number(paraRst);
	ui.serialrstEd->setText(strSerirst);
	ui.paralrstEd->setText(strPararst);

	QString strSpeedup = QString::number(speedup);
	ui.speedupEd->setText(strSpeedup);

}

double PI::serialPI()
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

double PI::multiPI()
{
	double pi = 0.0, x, y;
	int count = 0;
	omp_set_num_threads(numThread);
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
