using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BDCommon.Utils;

namespace BlackDesertGame.Managers.Tasks
{
    /* *
     * @author Maxes727
     */
    public static class TaskManager
    {
        public static List<GameTask> Tasks = new List<GameTask>();
        private static readonly object LockTasks = new object();
        private static Thread _mainTaskThread;

        public static void Init()
        {
            _mainTaskThread = new Thread(() =>
            {
                while (GameServer.IsWorked)
                {
                    lock (LockTasks)
                        foreach (GameTask task in Tasks)
                        {
                            if (task.WorkInterval == 0)
                            {
                                task.Task();
                                Tasks.Remove(task);
                            }
                            else
                            {
                                if (Funcs.GetRoundedUtc() - task.LastWorkTime > task.WorkInterval)
                                {
                                    task.Task();
                                    task.LastWorkTime = Funcs.GetRoundedUtc();
                                }
                            }
                        }
                    Thread.Sleep(1000);
                }
            });

            _mainTaskThread.Start();
        }

        public static void RemoveTask(Action action)
        {
            lock (LockTasks)
            {
                GameTask task = Tasks.FirstOrDefault(p => p.Task == action);

                if (task != null)
                    Tasks.Remove(task);
            }
        }

        public static void AddTask(Action action, int interval = 0)
        {
            lock (LockTasks)
            {
                if (Tasks.FirstOrDefault(p => p.Task == action) != null)
                    return;
                Tasks.Add(new GameTask(interval, action));
            }
        }
    }

    public class GameTask
    {
        public Action Task { get; private set; }

        public int LastWorkTime { get; set; }

        public int WorkInterval { get; private set; }

        public GameTask(int interval, Action action)
        {  
            LastWorkTime = Funcs.GetRoundedUtc();
            WorkInterval = interval;
            Task = action;
        }
    }
}
