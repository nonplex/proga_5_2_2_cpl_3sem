using System; // Импортируем пространство имен System для работы с классами и методами стандартной библиотеки .NET
using System.Drawing; // Импортируем пространство имен System.Drawing для работы с графикой и изображениями
using System.Windows.Forms; // Импортируем пространство имен System.Windows.Forms для работы с формами и элементами управления

namespace proga_5_2_2_cpl_3sem
{ // Определяем пространство имен для нашего проекта

    public partial class Form1 : Form
    { // Определяем класс Form1, который наследуется от класса Form

        private PictureBox background; // Создаем приватное поле PictureBox для фона
        private Bitmap buffered_bitmap; // Создаем приватное поле Bitmap для буферизованного изображения
        private Color border, fill, animation; // Создаем приватные поля Color для хранения цветов контура, заливки и анимации
        private bool drawning_mode; // Создаем приватное поле bool для хранения режима рисования
        private Button choose_color_button, start_timer_button; // Создаем приватные поля Button для кнопок "Сменить цвет" и "Начать анимацию"
        private TextBox timer_delay; // Создаем приватное поле TextBox для ввода задержки анимации
        private System.Windows.Forms.Timer timer; // Создаем приватное поле Timer для управления анимацией
        private int tick_counter; // Создаем приватное поле int для подсчета тиков таймера

        public Form1() // Определяем конструктор класса Form1
        {
            InitializeComponent(); // Инициализируем компоненты формы

            drawning_mode = false; // Устанавливаем начальное значение режима рисования в false
            border = Color.Black; // Устанавливаем начальное значение цвета контура в черный
            fill = Color.White; // Устанавливаем начальное значение цвета заливки в белый
            animation = Color.White; // Устанавливаем начальное значение цвета анимации в белый

            this.Size = new Size(1100, 800); // Устанавливаем размер формы 1100x800 пикселей
            this.Text = "Ракета"; // Устанавливаем текст заголовка формы "Ракета"

            background = new PictureBox(); // Создаем объект PictureBox для фона
            background.Location = new Point(15, 15); // Устанавливаем местоположение фона на координате (15, 15)
            background.Size = new Size(this.Width - 45, this.Height - 70); // Устанавливаем размер фона, чтобы он занимал большую часть формы
            drawImage(); // Вызываем метод drawImage() для отрисовки фона
            this.Controls.Add(background); // Добавляем фон в форму

            choose_color_button = new Button(); // Создаем объект Button для кнопки "Сменить цвет"
            choose_color_button.Location = new Point(20, 20); // Устанавливаем местоположение кнопки на координате (20, 20)
            choose_color_button.Size = new Size(140, 25); // Устанавливаем размер кнопки 140x25 пикселей
            choose_color_button.Text = "Сменить цвет"; // Устанавливаем текст кнопки "Сменить цвет"
            choose_color_button.Click += choose_color_button_Click; // Привязываем событие нажатия кнопки к методу choose_color_button_Click()
            this.Controls.Add(choose_color_button); // Добавляем кнопку в форму
            choose_color_button.BringToFront(); // Помещаем кнопку поверх других элементов формы

            start_timer_button = new Button(); // Создаем объект Button для кнопки "Начать анимацию"
            start_timer_button.Location = new Point(20, 50); // Устанавливаем местоположение кнопки на координате (20, 50)
            start_timer_button.Size = new Size(140, 25); // Устанавливаем размер кнопки 140x25 пикселей
            start_timer_button.Text = "Начать анимацию"; // Устанавливаем текст кнопки "Начать анимацию"
            start_timer_button.Click += start_timer_button_Click; // Привязываем событие нажатия кнопки к методу start_timer_button_Click()
            this.Controls.Add(start_timer_button); // Добавляем кнопку в форму
            start_timer_button.BringToFront(); // Помещаем кнопку поверх других элементов формы

            timer_delay = new TextBox(); // Создаем объект TextBox для ввода задержки анимации
            timer_delay.Location = new Point(170, 53); // Устанавливаем местоположение текстбокса на координате (170, 53)
            timer_delay.Size = new Size(40, 25); // Устанавливаем размер текстбокса 40x25 пикселей
            timer_delay.Text = "500"; // Устанавливаем начальное значение текстбокса "500"
            this.Controls.Add(timer_delay); // Добавляем текстбокс в форму
            timer_delay.BringToFront(); // Помещаем текстбокс поверх других элементов формы

            timer = new System.Windows.Forms.Timer(); // Создаем объект Timer для управления анимацией
            timer.Tick += timer_Tick; // Привязываем событие тика таймера к методу timer_Tick()
        }

        private void choose_color_button_Click(object sender, EventArgs e) // Определяем метод choose_color_button_Click() для обработки нажатия кнопки "Сменить цвет"
        {
            chooseColors(); // Вызываем метод chooseColors() для выбора цветов контура, заливки и анимации
            drawImage(); // Вызываем метод drawImage() для отрисовки фона с новыми цветами
        }

        private void start_timer_button_Click(object sender, EventArgs e) // Определяем метод start_timer_button_Click() для обработки нажатия кнопки "Начать анимацию"
        {
            int delay = 500; // Устанавливаем начальное значение задержки анимации в 500 миллисекунд
            try
            {
                int temp_delay = delay; // Создаем временную переменную для хранения начального значения задержки
                delay = int.Parse(timer_delay.Text); // Получаем значение задержки из текстбокса и преобразуем его в int
                if (delay <= 0) // Если значение задержки меньше или равно нулю
                {
                    delay = temp_delay; // Восстанавливаем начальное значение задержки
                    throw new Exception(); // Генерируем исключение
                }
            }
            catch (Exception) // Обрабатываем исключение
            {
                MessageBox.Show("Вы ввели невозможное значение!", "Неверное значение", MessageBoxButtons.OK); // Выводим сообщение об ошибке
            }
            timer.Interval = delay; // Устанавливаем интервал таймера в заданную задержку
            tick_counter = 0; // Сбрасываем счетчик тиков таймера
            timer.Start(); // Запускаем таймер
        }

        private void timer_Tick(object sender, EventArgs e) // Определяем метод timer_Tick() для обработки тика таймера
        {
            if (tick_counter > 10) // Если счетчик тиков таймера больше 10
            {
                timer.Stop(); // Останавливаем таймер
                return; // Прерываем выполнение метода
            }
            if (drawning_mode) // Если режим рисования равен true
            {
                drawning_mode = false; // Устанавливаем режим рисования в false
            }
            else
            {
                drawning_mode = true; // Устанавливаем режим рисования в true
            }
            drawAnimation(); // Вызываем метод drawAnimation() для отрисовки анимации
            tick_counter++; // Увеличиваем счетчик тиков таймера на 1
        }

        private void chooseColors() // Определяем метод chooseColors() для выбора цветов контура, заливки и анимации
        {
            ColorDialog dialog = new ColorDialog(); // Создаем объект ColorDialog для выбора цвета
            MessageBox.Show("Выберите цвет контура.", "Цвет контура", MessageBoxButtons.OK); // Выводим сообщение для выбора цвета контура
            if (dialog.ShowDialog() != DialogResult.OK) // Если пользователь нажал кнопку "Отмена" в диалоге выбора цвета
            {
                border = Color.Blue; // Устанавливаем цвет контура в синий (значение по умолчанию)
            }
            else
            {
                border = dialog.Color; // Устанавливаем цвет контура в выбранный пользователем цвет
            }

            MessageBox.Show("Выберите цвет ракеты.", "Цвет фигур", MessageBoxButtons.OK); // Выводим сообщение для выбора цвета заливки
            if (dialog.ShowDialog() != DialogResult.OK) // Если пользователь нажал кнопку "Отмена" в диалоге выбора цвета
            {
                fill = Color.Cyan; // Устанавливаем цвет заливки в голубой (значение по умолчанию)
            }
            else
            {
                fill = dialog.Color; // Устанавливаем цвет заливки в выбранный пользователем цвет
            }

            MessageBox.Show("Выберите цвет огня.", "Цвет огня", MessageBoxButtons.OK); // Выводим сообщение для выбора цвета анимации
            if (dialog.ShowDialog() != DialogResult.OK) // Если пользователь нажал кнопку "Отмена" в диалоге выбора цвета
            {
                animation = Color.Pink; // Устанавливаем цвет анимации в розовый (значение по умолчанию)
            }
            else
            {
                animation = dialog.Color; // Устанавливаем цвет анимации в выбранный пользователем цвет
            }
        }

        private void drawImage() // Определяем метод drawImage() для отрисовки фона с выбранными цветами
        {
            Image image = Image.FromFile("C:\\Users\\Vladislav\\Desktop\\3 sem\\proga v21 semestr 3\\5 lab\\qt4.jpeg"); // Загружаем изображение фона из файла
            Bitmap bitmap = new Bitmap(background.Width, background.Height); // Создаем битмап для фона с размером, равным размеру фона
            Graphics g = Graphics.FromImage((System.Drawing.Image)bitmap); // Создаем объект Graphics для рисования на битмапе
            g.DrawImage(image, 0, 0, background.Width, background.Height); // Рисуем изображение фона на битмапе
            g.Dispose(); // Освобождаем ресурсы объекта Graphics
            background.Image = (System.Drawing.Image)bitmap; // Устанавливаем битмап в качестве изображения фона
            drawOnImage(); // Вызываем метод drawOnImage() для отрисовки ракеты на битмапе
        }

        private void drawOnImage() // Определяем метод drawOnImage() для отрисовки ракеты на битмапе
        {
            Bitmap bitmap = new Bitmap(background.Image); // Создаем битмап для фона с размером, равным размеру фона
            Graphics g = Graphics.FromImage(bitmap); // Создаем объект Graphics для рисования на битмапе
            Pen pen = new Pen(border, 2); // Создаем объект Pen для рисования контура ракеты с заданным цветом и шириной
            SolidBrush brush = new SolidBrush(fill); // Создаем объект SolidBrush для заливки ракеты с заданным цветом

            // Овал ракеты
            SolidBrush rocketBrush = new SolidBrush(fill); // Создаем объект SolidBrush для заливки ракеты с заданным цветом
            Pen rocketPen = new Pen(border, 2); // Создаем объект Pen для рисования контура ракеты с заданным цветом и шириной
            g.FillEllipse(rocketBrush, 300, 300, 250, 100); // Заливаем овал ракеты с заданными размером и цветом
            g.DrawEllipse(rocketPen, 300, 300, 250, 100); // Рисуем контур овала ракеты с заданными размером и цветом

            // Верхний треугольник
            Point[] upperTriangle = { // Массив точек для верхнего треугольника ракеты
                new Point(400, 300 - 50),
                new Point(400+50, 300 - 50),
                new Point(300+125, 300)};
            g.FillPolygon(brush, upperTriangle); // Заливаем верхний треугольник ракеты с заданным цветом
            g.DrawPolygon(pen, upperTriangle); // Рисуем контур верхнего треугольника ракеты с заданным цветом и шириной

            // Нижний треугольник
            Point[] lowerTriangle = {
                new Point(400, 300 + 50 + 100),
                new Point(450, 300 + 50 + 100),
                new Point(300+125, 300 + 100)
            };
            g.FillPolygon(brush, lowerTriangle); // Заливаем нижний треугольник ракеты с заданным цветом
            g.DrawPolygon(pen, lowerTriangle); // Рисуем контур нижнего треугольника ракеты с заданным цветом и шириной

            // Илюминатор
            g.FillEllipse(new SolidBrush(Color.Yellow), 425 - 75, 350 - 25, 50, 50); // Заливаем илюминатор ракеты с заданными размером и цветом

            g.Dispose(); // Освобождаем ресурсы объекта Graphics
            buffered_bitmap = bitmap; // Сохраняем битмап с нарисованной ракетой в поле buffered_bitmap
            drawAnimation(); // Вызываем метод drawAnimation() для отрисовки анимации на битмапе
        }

        private void drawAnimation() // Определяем метод drawAnimation() для отрисовки анимации на битмапе
        {
            background.Image = (Image)buffered_bitmap; // Устанавливаем битмап с нарисованной ракетой в качестве изображения фона
            Bitmap bitmap = new Bitmap(background.Image); // Создаем битмап для фона с размером, равным размеру фона
            Graphics g = Graphics.FromImage(bitmap); // Создаем объект Graphics для рисования на битмапе
            Pen pen = new Pen(border, 2); // Создаем объект Pen для рисования контура анимации с заданным цветом и шириной
            SolidBrush brush = new SolidBrush(animation); // Создаем объект SolidBrush для заливки анимации с заданным цветом

            if (drawning_mode) // Если режим рисования равен true
            {
                Point[] Ogonn = {

                    new Point(575, 280), // Правая нижняя точка эллипса огня
                    new Point(500, 350), // Вершина треугольника огня, не лежащая на эллипсе
                    new Point(575, 420) // Вершина треугольника огня, не лежащая на эллипсе
                         };

                g.FillPolygon(brush, Ogonn); // Заливаем треугольник огня с заданным цветом
                g.DrawPolygon(pen, Ogonn); // Рисуем контур треугольника огня с заданным цветом и шириной
            }
            else
            {
                Point[] Ogon = {

                    new Point(560, 310), // Правая нижняя точка эллипса огня
                    new Point(500, 350), // Вершина треугольника огня, не лежащая на эллипсе
                    new Point(560, 390) // Вершина треугольника огня, не лежащая на эллипсе
                         };

                g.FillPolygon(brush, Ogon); // Заливаем треугольник огня с заданным цветом
                g.DrawPolygon(pen, Ogon); // Рисуем контур треугольника огня с заданным цветом и шириной

            }
            background.Image = (Image)bitmap; // Устанавливаем битмап с нарисованной анимацией в качестве изображения фона
        }
    }
}