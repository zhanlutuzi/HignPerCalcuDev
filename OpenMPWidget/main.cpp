#include "OpenMPWidget.h"
#include <QtWidgets/QApplication>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    OpenMPWidget w;
    w.show();
    return a.exec();
}
