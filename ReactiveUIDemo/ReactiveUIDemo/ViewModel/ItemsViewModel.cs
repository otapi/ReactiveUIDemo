using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUIDemo.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUIDemo.ViewModel
{
    public class ItemsViewModel : ViewModelBase
    {
        private SourceList<Todo> todos { get; } = new SourceList<Todo>();
        private readonly IObservableCollection<Todo> targetCollection = new ObservableCollectionExtended<Todo>();
        public IObservableCollection<Todo> Todos => targetCollection;

        private Todo selectedTodo;
        public Todo SelectedTodo
        {
            get => selectedTodo; 
            set => this.RaiseAndSetIfChanged(ref selectedTodo , value);
        }


        private ObservableAsPropertyHelper<bool> canAdd;
        public bool CanAdd => canAdd?.Value ?? false;

        
        private string todoTitle;
        public string TodoTitle
        {
            get { return todoTitle; }
            set {this.RaiseAndSetIfChanged(ref todoTitle, value); }
        }

        public ReactiveCommand<Unit, Todo> AddItem { get; private set; }

        async Task<Todo> AddItemImpl()
        {
            if (CanAdd)
            {
                var newItem = new Todo() { Title = TodoTitle };
                Todos.Add(newItem);

                TodoTitle = string.Empty;
                return newItem;
            }
            else
            {
                return null;
            }
        }
        public ItemsViewModel(IScreen hostScreen = null) : base(hostScreen)
        {
   
            AddItem = ReactiveCommand.CreateFromObservable((Unit unit) => Observable.StartAsync(AddItemImpl));

      
            todos.Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(targetCollection)
            .Subscribe();
            
            this.WhenAnyValue(x => x.TodoTitle,
                title => 
                !String.IsNullOrEmpty(title)).ToProperty(this, x => x.CanAdd, out canAdd);

            todos.Add(new Todo { IsDone = false, Title = "Go to Sleep" });
            todos.Add(new Todo { IsDone = false, Title = "Go get some dinner" });
            todos.Add(new Todo { IsDone = false, Title = "Watch GOT" });
            todos.Add(new Todo { IsDone = false, Title = "Code code and code!!!!" });

            ///Lets detect when ever a todo Item is marked as done 
            ///IF it is, it is sent to the bottom of the list
            ///Else nothing happens
            
            todos.Connect()
                .WhenAnyPropertyChanged()
                .Subscribe(x =>
                {
                    if (x.IsDone)
                    {
                        todos.Remove(x);
                        todos.Add(x);
                    }
                });
        }
    }
}
