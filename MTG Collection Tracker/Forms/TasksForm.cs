﻿using KW.WinFormsUI.Docking;

namespace MTG_Librarian
{
    public partial class TasksForm : DockForm
    {
        public TaskManager TaskManager { get; private set; }

        public TasksForm()
        {
            InitializeComponent();
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockBottom;
        }

        public void InitializeTaskManager()
        {
            TaskManager = new TaskManager(Globals.Forms.MainForm.TasksLabel, Globals.Forms.MainForm.TasksProgressBar);
        }
    }
}
