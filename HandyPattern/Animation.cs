using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace HandyPattern
{
    class Animation
    {
        private Window _mainWindow;
        private double _mainWindowMaxHeight;
        private double _mainWindowCloseTargetSize;

        private const double MAIN_WINDOW_ANIMATION_SPEED = 0.2;

        
        public Animation(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _mainWindowMaxHeight = mainWindow.ActualHeight;
            _mainWindowCloseTargetSize = mainWindow.DropDownButton.ActualHeight;
        }
        public DoubleAnimation CloseMainWorkspaceAnimation()
        {
            return MainWorkspaceAnimation(_mainWindow.ActualHeight, _mainWindowCloseTargetSize, MAIN_WINDOW_ANIMATION_SPEED);
  
        }

        public DoubleAnimation OpenMainWorkspaceAnimation()
        {
            return MainWorkspaceAnimation(_mainWindowCloseTargetSize, _mainWindowMaxHeight, MAIN_WINDOW_ANIMATION_SPEED);
        }

        private DoubleAnimation MainWorkspaceAnimation(double currentHeight, double targetHeight, double duration)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = currentHeight;
            animation.To = targetHeight;
            animation.Duration = TimeSpan.FromSeconds(duration);
            return animation;
        }
    }
}
