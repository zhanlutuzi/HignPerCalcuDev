#include "OpenMPWidget.h"

OpenMPWidget::OpenMPWidget(QWidget *parent)
    : QMainWindow(parent)
{
    ui.setupUi(this);
    pMatrix = new Matrix();
    pPI = new PI();
    connect(ui.matrixbtn, SIGNAL(clicked(bool)) , this, SLOT(openMatrixWidget()));
    connect(ui.PIbtn, SIGNAL(clicked(bool)), this, SLOT(openPiWidget()));
}

OpenMPWidget::~OpenMPWidget()
{
    delete pMatrix;
    delete pPI;
    pMatrix = nullptr;
    pPI = nullptr;
}

void OpenMPWidget::openMatrixWidget()
{
    pMatrix->show();
}

void OpenMPWidget::openPiWidget()
{
    pPI->show();
}
