#pragma once

#include <QtWidgets/QMainWindow>
#include <QDebug>
#include "ui_OpenMPWidget.h"
#include "Matrix.h"
#include "PI.h"

class OpenMPWidget : public QMainWindow
{
    Q_OBJECT

public:
    OpenMPWidget(QWidget *parent = nullptr);
    ~OpenMPWidget();

private slots :
	void openMatrixWidget();
    void openPiWidget();

private:
    Ui::OpenMPWidgetClass ui;
    Matrix *pMatrix;
    PI *pPI;


};
