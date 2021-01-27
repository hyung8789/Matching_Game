using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Matching_Game
{
    public partial class Form1 : Form
    {
        Random rand = new Random();
        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        }; //한 쌍씩 동일한 아이콘 리스트

        //사용자가 클릭한 라벨 추적 (참조 변수)
        Label first_clicked_label = null;
        Label second_clicked_label = null;

        uint global_timer_count = 0;

        private void assign_icons_to_squares()
        {
            foreach (Control control in tableLayoutPanel1.Controls) //레이아웃 패널의 각 컨트롤에 대하여
            {
                Label icon_label = control as Label; //Label형으로 캐스팅

                if (icon_label != null)
                {
                    int rand_num = rand.Next(icons.Count);
                    icon_label.Text = icons[rand_num];
                    icon_label.ForeColor = icon_label.BackColor; //아이콘을 숨기기 위해 색을 같게 한다.
                    icons.RemoveAt(rand_num); //사용 된 아이콘 리스트에서 제거
                }
            }
        }
        private void chk_for_winner() //게임 종료 판별
        {
            //모든 라벨이 판별되었으면 종료
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label icon_label = control as Label;

                if (icon_label != null)
                {
                    if (icon_label.ForeColor == icon_label.BackColor) //하나라도 판별되지 않은 것이 있으면
                        return;
                }
            }

            global_timer.Stop();

            MessageBox.Show("모든 아이콘을 맞췄습니다.\n경과 시간 : " + global_timer_count + "s", "축하합니다!");
            Close();
        }

        public Form1()
        {
            InitializeComponent();
           
            assign_icons_to_squares();
        }

        private void timer_Tick(object sender, EventArgs e) //라벨에 대해 클릭 시간 추적 위한 타이머 이벤트 처리부
        {
            //시간 초과 시(750ms 후) 현재 클릭 된 라벨들 모두 숨기고 초기화
            timer.Stop();

            first_clicked_label.ForeColor = first_clicked_label.BackColor;
            second_clicked_label.ForeColor = second_clicked_label.BackColor;

            first_clicked_label = second_clicked_label = null;
        }

        private void label_Click(object sender, EventArgs e) //라벨의 클릭 이벤트 처리부
        {
            if (timer.Enabled) //라벨에 대해 클릭 시간 추적 위한 타이머가 이미 작동 중이면 첫 번째와 두 번째 클릭 후 더 이상 클릭 못하도록 한다.
                return;
            
            Label clicked_label = sender as Label; //Label로 캐스팅
            
            if(clicked_label != null)
            {
                if (clicked_label.ForeColor == Color.Black) //이미 클릭 된 라벨일 경우
                    return;
            
                if(first_clicked_label == null) //사용자가 첫 번째로 클릭 한 라벨 추적
                {
                    first_clicked_label = clicked_label;
                    first_clicked_label.ForeColor = Color.Black;

                    return;
                }

                //두 번째로 클릭 한 라벨 추적
                second_clicked_label = clicked_label;
                second_clicked_label.ForeColor = Color.Black;

                if(first_clicked_label.Text == second_clicked_label.Text) //사용자가 올바른 한 쌍을 클릭하였을 경우
                {
                    first_clicked_label = second_clicked_label = null; //해당 라벨들 더 이상 추적하지 않음
                    chk_for_winner(); //게임 종료 판별

                    return;
                }

                timer.Start(); //사용자가 두 개의 라벨을 클릭 후 라벨에 대해 클릭 시간 추적 위한 타이머 시작
            }
        }

        private void global_timer_Tick(object sender, EventArgs e) //게임 경과 시간 타이머 이벤트 처리부
        {
            global_timer_label.Text = "경과 시간 : " + global_timer_count++ + "s";
        }
    }
}
