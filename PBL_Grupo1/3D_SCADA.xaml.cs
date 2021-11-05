using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Collections;
using System.Threading;
using System.Windows.Threading;

namespace PBL_Grupo1
{
    /// <summary>
    /// Lógica interna para _3D_SCADA.xaml
    /// </summary>

    public delegate void tela_scada();

    public partial class _3D_SCADA : Window
    {
        private static Timer timer;

        Model3DGroup guardar_carril = null;
        Model3DGroup guardar_pallet_enCarril = null;


        Model3DGroup vacio = null;
        Double parar_int;

        DoubleAnimation CARRIL_ANIMACION = new DoubleAnimation();


        Storyboard Fcarril = new Storyboard();

        BitArray bitsEstacion1;

        public _3D_SCADA()
        {
            InitializeComponent();
            //bitsEstacion1 = bits;

            vacio = new Model3DGroup();
            parar_int = 130;

            guardar_carril = (Model3DGroup)Model.Children[0];
            Model.Children[0] = vacio;

            guardar_pallet_enCarril = (Model3DGroup)CARRIL_PALLET.Children[1];
            CARRIL_PALLET.Children[1] = vacio;


            Storyboard fDir = (Storyboard)FindResource("Cinta_mover_dir");
            fDir.Begin();
            fDir.Pause();


            Storyboard Fcarril = (Storyboard)FindResource("CARRIL_STORY");
            Fcarril.Begin();
            Fcarril.Pause();
            /*
                        CARRIL_ANIMACION.Duration = TimeSpan.FromSeconds(2);
                        CARRIL_ANIMACION.From = 0.0;
                        CARRIL_ANIMACION.To = 30.0;
                        CARRIL_STORY.Children.Add(CARRIL_ANIMACION);

                        Storyboard.SetTargetName(CARRIL_ANIMACION, nameof(MOVER_CARRIL));
                        Storyboard.SetTargetProperty(CARRIL_ANIMACION, new PropertyPath(TranslateTransform3D.OffsetXProperty));

                                    fIzq_color1.Brush = Brushes.Transparent;   // poner trasparente la flecha a la izq
                        fIzq_color2.Brush = Brushes.Transparent;
                        fIzq_color3.Brush = Brushes.Transparent;
                        fIzq_color4.Brush = Brushes.Transparent;

            */

            fIzq_color1.Brush = Brushes.Transparent;   // poner trasparente la flecha a la izq
            fIzq_color2.Brush = Brushes.Transparent;
            fIzq_color3.Brush = Brushes.Transparent;
            fIzq_color4.Brush = Brushes.Transparent;


            

        }

        public void setBits(BitArray bits)
        {
            this.bitsEstacion1 = bits;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(procesaDatosEstacion);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void procesaDatosEstacion(object sender, EventArgs e)
        {
            if(bitsEstacion1!= null)
            {
                palet_extremo_izquierdo.IsChecked = bitsEstacion1[6];
                bajar_cilindro_elevador.IsChecked = bitsEstacion1[25];
                subir_cilindro_elevador.IsChecked = bitsEstacion1[24];
                Arrancar_cinta_derecha.IsChecked = bitsEstacion1[13];
            }
        }


        private void Cilindro_elevador_arriba_Checked(object sender, RoutedEventArgs e)
        {

        }


    /*    private void parar_pallet_Checked(object sender, RoutedEventArgs e)
        {

                guardarCarril = (Model3DGroup)Model.Children[24];
                Model.Children[24] = vacio;
        
        }

        private void parar_pallet_Unchecked(object sender, RoutedEventArgs e)
        {
            Model.Children[24] = guardarCarril;
            guardarCarril = null;
        }
    */
        private void Arrancar_cinta_derecha_Checked(object sender, RoutedEventArgs e)
        {
            Storyboard fDir = (Storyboard)FindResource("Cinta_mover_dir");
            Storyboard Fcarril = (Storyboard)FindResource("CARRIL_STORY");
          
            fDir.Resume();

            if (guardar_carril == Model.Children[0])
            {
                Fcarril.Resume();
            }

           if (palet_extremo_izquierdo.IsChecked==true)
            {
                Model.Children[0] = guardar_carril;
                Fcarril.Resume();
            }

        }

        private void Arrancar_cinta_derecha_Unchecked(object sender, RoutedEventArgs e)
        {
            Storyboard fDir = (Storyboard)FindResource("Cinta_mover_dir");
            fDir.Pause();
            Storyboard Fcarril = (Storyboard)FindResource("CARRIL_STORY");
            Fcarril.Pause();

        }

        private void palet_extremo_izquierdo_Checked(object sender, RoutedEventArgs e)
        {
            Storyboard fDir = (Storyboard)FindResource("Cinta_mover_dir");
            Storyboard Fcarril = (Storyboard)FindResource("CARRIL_STORY");

                Model.Children[24] = guardar_carril;

                if (!fDir.GetIsPaused())
                {
                    Fcarril.Resume();
                }
            
        }

        private void Hay_carril_Checked(object sender, RoutedEventArgs e)
        {

            Storyboard fDir = (Storyboard)FindResource("Cinta_mover_dir");
            Storyboard Fcarril = (Storyboard)FindResource("CARRIL_STORY");

            if (!fDir.GetIsPaused())
            {
                Fcarril.Resume();
            }

        }




        private void bajar_cilindro_elevador_Checked_1(object sender, RoutedEventArgs e)
        {
            Storyboard ALMAZEN_BAJAR = (Storyboard)FindResource("ALMAZEN_STORY_BAJAR");
            ALMAZEN_BAJAR.Begin();
        }

        private void subir_cilindro_elevador_Checked(object sender, RoutedEventArgs e)
        {
            Storyboard ALMAZEN_SUBIR = (Storyboard)FindResource("ALMAZEN_STORY_SUBIR");
            ALMAZEN_SUBIR.Begin();
        }

        private void Hay_pallet_Checked(object sender, RoutedEventArgs e)
        {
            CARRIL_PALLET.Children[1] = guardar_pallet_enCarril;
        }

        private void Hay_pallet_Unchecked(object sender, RoutedEventArgs e)
        {
            CARRIL_PALLET.Children[1] = vacio;
        }



        private void MainViewport3D_MouseWheel(object sender, MouseWheelEventArgs e)
        {

          /*
           * var st = ;
                var tt = GetTranslateTransform(child);

                double zoom = e.Delta > 0 ? .2 : -.2;
                if (!(e.Delta > 0) && (st.ScaleX < .4 || st.ScaleY < .4))
                    return;

                Point relative = e.GetPosition(child);
                double absoluteX;
                double absoluteY;

                absoluteX = relative.X * st.ScaleX + tt.X;
                absoluteY = relative.Y * st.ScaleY + tt.Y;

                st.ScaleX += zoom;
                st.ScaleY += zoom;

                tt.X = absoluteX - relative.X * st.ScaleX;
                tt.Y = absoluteY - relative.Y * st.ScaleY;
          */
        }
    }         

    
}

/*
public void Initialize(UIElement element)
{
    this.child = element;
    if (child != null)
    {
        TransformGroup group = new TransformGroup();
        ScaleTransform st = new ScaleTransform();
        group.Children.Add(st);
        TranslateTransform tt = new TranslateTransform();
        group.Children.Add(tt);
        child.RenderTransform = group;
        child.RenderTransformOrigin = new Point(0.0, 0.0);
        this.MouseWheel += child_MouseWheel;
    }
}
private void child_MouseWheel(object sender, MouseWheelEventArgs e)
{
    */