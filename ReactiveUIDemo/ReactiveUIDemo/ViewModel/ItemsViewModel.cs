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
        private SourceList<Todo> _todos { get; } = new SourceList<Todo>();
        private readonly IObservableCollection<Todo> _targetCollection = new ObservableCollectionExtended<Todo>();
        public IObservableCollection<Todo> Todos => _targetCollection;

        private Todo _selectedTodo;
        public Todo SelectedTodo
        {
            get => _selectedTodo; 
            set => this.RaiseAndSetIfChanged(ref _selectedTodo , value);
        }

        private ObservableAsPropertyHelper<bool> _canAdd;
        public bool CanAdd => _canAdd?.Value ?? false;

        private string _todoTitl;
        public string TodoTitle
        {
            get { return _todoTitl; }
            set {this.RaiseAndSetIfChanged(ref _todoTitl, value); }
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

            _todos.Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(_targetCollection)
            .Subscribe();
            
            this.WhenAnyValue(x => x.TodoTitle,
                title => 
                !String.IsNullOrEmpty(title)).ToProperty(this, x => x.CanAdd, out _canAdd);

            _todos.Add(new Todo { IsDone = false, Title = "Go to Sleep" });
            _todos.Add(new Todo { IsDone = false, Title = "Go get some dinner" });
            _todos.Add(new Todo { IsDone = false, Title = "Watch GOT" });
            _todos.Add(new Todo { IsDone = false, Title = "Code code and code!!!!" });

            ///Lets detect when ever a todo Item is marked as done 
            ///IF it is, it is sent to the bottom of the list
            ///Else nothing happens
            
            _todos.Connect()
                .WhenAnyPropertyChanged()
                .Subscribe(x =>
                {
                    if (x.IsDone)
                    {
                        _todos.Remove(x);
                        _todos.Add(x);
                    }
                });
        }
    }
}
