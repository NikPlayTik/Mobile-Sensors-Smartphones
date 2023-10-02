using System;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Diagnostics;
using Xamarin.Forms.PlatformConfiguration;
using System.Threading;
using System.Collections.Generic;

namespace Mobile_Sensors
{ 
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Acselerometr_Start();
            Gyroscope_Start();
            Magnetometr_Start();
            AlertCalibration();
            ButtonHelp_Clicked();
        }

        async void AlertCalibration()
        {
            await DisplayAlert("Внимание", "Значения датчика магнитометра должно быть в пределах 40-60, если значения выше, то нужна калибровка датчика. " +
                "Чтобы откалибровать датчик, покрутите телефон знаком бесконечности 10 секунд.", "Продолжить");
        }
        void OnAccelerometer(object sender, AccelerometerChangedEventArgs e)
        {
            
                //получаем значения по координатам xyz
                double x = e.Reading.Acceleration.X;
                double y = e.Reading.Acceleration.Y;
                double z = e.Reading.Acceleration.Z;

                //получаем угол поворота по оси xyz в градусах
                double x_roll = Math.Atan2(x, Math.Sqrt(y * y + z * z)) * 180 / Math.PI;
                double y_roll = Math.Atan2(y, Math.Sqrt(x * x + z * z)) * 180 / Math.PI;
                double z_roll = Math.Atan2(Math.Sqrt(x * x + y * y), z) * 180 / Math.PI;

                //выводим значения на экран {x:F2} - это форматированная строка разрядность F2
                znachAcselerometr.Text = $"X: {x:F1} - Угол поворота: {x_roll:F0}\nY: {y:F1} - Угол поворота: {y_roll:F0}\nZ: {z:F1} - Угол поворота: {z_roll:F0}";
        }
        async void Acselerometr_Start()
        {
            try
            {
                Accelerometer.ReadingChanged += OnAccelerometer;
                Accelerometer.Start(SensorSpeed.UI);
            }
            catch
            {
                await DisplayAlert("Ошибка при анализе", "Возможно датчика акселерометра нету в данном устройстве", "Продолжить");
                znachAcselerometr.Text = $"X: 0 - Угол поворота: 0\nY: 0 - Угол поворота: 0\nZ: 0 - Угол поворота: 0";
            }
        }
        
        void OnGyroscope(object sender, GyroscopeChangedEventArgs e)
        {
                // Получаем значения гироскопа по осям x, y и z
                double x = e.Reading.AngularVelocity.X;
                double y = e.Reading.AngularVelocity.Y;
                double z = e.Reading.AngularVelocity.Z;

                // Отображаем значения на экране
                znachGyroscope.Text = $"X: {x:F1} Y: {y:F1} Z: {z:F1}";
        }
        async void Gyroscope_Start()
        {
            try
            {
                Gyroscope.ReadingChanged += OnGyroscope;
                Gyroscope.Start(SensorSpeed.UI);
            }
            catch
            {
                await DisplayAlert("Ошибка при анализе", "Возможно датчика гироскоп нету в данном устройстве", "Продолжить");
                znachGyroscope.Text = $"X: 0 Y: 0 Z: 0";
            }
        }

        void OnMagnetometer(object sender, MagnetometerChangedEventArgs e)
        {
            // Получаем значения магнитометра по осям x, y и z
            double microTeslaX = e.Reading.MagneticField.X;

            double microTeslaY = e.Reading.MagneticField.Y;

            double microTeslaZ = e.Reading.MagneticField.Z;

            // Формула для вычисления общего значения магнитного поля
            double totalMicroTesla = Math.Sqrt(microTeslaX * microTeslaX + microTeslaY * microTeslaY + microTeslaZ * microTeslaZ);

            // Отображаем общего значения магнитного поля земли на экране
            znachMagnitometr.Text = $"{totalMicroTesla:F0} μT";

            // Вычисление угла направления компаса
            double angle = Math.Atan2(microTeslaY, microTeslaX) * (180 / Math.PI);

            //Корректировка отрицательных значений угла
            if (angle < 0)
            {
                angle += 360;
            }

            // Обновление вращения стрелки компаса
            compasImage.Rotation = angle;
        }
        async void Magnetometr_Start()
        {
            try
            {
                Magnetometer.ReadingChanged += OnMagnetometer;
                Magnetometer.Start(SensorSpeed.UI);
            }
            catch
            {
                await DisplayAlert("Ошибка при анализе", "Возможно датчика магнитного поля нету в данном устройстве", "Продолжить");
                znachMagnitometr.Text = $"0 μT";
            }
        }

        void ButtonHelp_Clicked()
        {
            buttonHelp.Clicked += async (sender, e) =>
            {
                try
                {
                    await DisplayAlert("Справка по использованию датчиков устройства", "Главное окно программы. " +
                        "\nГлавное окно программы отображает все работающие датчики устройства и их вывод пользователю на экран. " +
                        "\n" +
                        "\nИспользование датчика акселерометра. " +
                        "\nЧтобы изменять показания датчика акселерометра, нужно повернуть устройство в разных направлениях и наблюдать изменения значений осей X, Y и Z. Ось X отвечает за горизонтальное движение устройства влево и вправо, ось Y – за вертикальное движение вверх и вниз, а ось Z – за движение вперед и назад. " +
                        "\n" +
                        "\nИспользование датчика гироскопа." +
                        "\nЧтобы изменить показания гироскопа, необходимо поворачивать устройство в разных направлениях и наблюдать изменения угловых скоростей вокруг осей X, Y и Z. Ось X отвечает за вращение устройства вокруг горизонтальной оси, ось Y – за вращение вокруг вертикальной оси, а ось Z – за вращение вокруг оси, направленной от лица пользователя к задней части устройства." +
                        "\n" +
                        "\nИспользование датчика магнитометра." +
                        "\nЧтобы получить показания магнитометра, необходимо приблизить устройство к магнитным объектам или проводить его движение в окружении магнитных полей. Магнитометр измеряет интенсивность магнитного поля и выражает ее в микротеслах (µT). При приближении к магнитному полю значения магнитометра увеличиваются, а при удалении они уменьшаются. Показания магнитометра могут использоваться для определения направления магнитного поля или для обнаружения магнитных объектов в окружающей среде." +
                        "\n" +
                        "\nИспользование компаса." +
                        "\nКомпас основан на данных, полученных от магнитометра. Он отображает направление магнитного поля и позволяет определить ориентацию устройства относительно магнитного севера. Когда устройство поворачивается вокруг своей оси, компас реагирует на изменения магнитного поля и вращается соответственно. Пользователь может использовать компас для определения своего направления или для навигации в пространстве. Значения компаса обычно выражаются в градусах, где 0 градусов соответствует северу, 90 градусов – востоку, 180 градусов – югу и 270 градусов – западу.", "Хорошо");
                }
                catch
                {
                    await DisplayAlert("Критическая ошибка", "Что-то пошло не так при выводе справки", "Хорошо");
                }
            };
            
        }
    }

}
