using ApplicationCore.Interfaces;
using System;
using System.Windows.Forms;

namespace ToDoApp
{
    public partial class Form1 : Form
    {
        private readonly IGroupTaskService _groupTaskService;

        public Form1(IGroupTaskService groupTaskService)
        {
            InitializeComponent();
            _groupTaskService = groupTaskService;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var data = await _groupTaskService.GetAllAsync();
            dataGridView1.DataSource = data;
        }
    }
}
