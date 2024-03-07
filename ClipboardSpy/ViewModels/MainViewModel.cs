using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClipboardSpy.ViewModels;

public class MainViewModel : ViewModelBase {
    public bool IsTextWrapped { get; set; } = true;
    public ObservableCollection<FormatViewModel> Items { get; set; } = [];
    public FormatViewModel SelectedItem { get; set; }
    public MainViewModel() { }

    public void SetDataObject(IDataObject ido) {
        Items.Clear();
        var formats = ido.GetDataFormats();
        foreach (string format in formats) {
            object data = ido.Get(format);
            Items.Add(new() {
                FormatName = format,
                FormatData = data
            });
        }
    }
    public ICommand ToggleTextWrappingCommand => ReactiveCommand.Create(() => {
        IsTextWrapped = !IsTextWrapped;
        this.RaisePropertyChanged(nameof(IsTextWrapped));
    });

    public ICommand ReadClipboardCommand => ReactiveCommand.Create(async () => {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime cda ||
            cda.MainWindow is not Window w ||
            w.Clipboard is not { } cb) {
            return;
        }
        DataObject ido = new DataObject();
        var formats = await cb.GetFormatsAsync();
        foreach (var format in formats) {
            var data = await cb.GetDataAsync(format);
            ido.Set(format, data);
        }
        SetDataObject(ido);
    });


    public ICommand ClearClipboardCommand => ReactiveCommand.Create(async () => {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime cda ||
            cda.MainWindow is not Window w ||
            w.Clipboard is not { } cb) {
            return;
        }
        Items.Clear();
        await cb.ClearAsync();
    });

    public ICommand ExitApplicationCommand => ReactiveCommand.Create(() => {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime cda) {
            return;
        }
        cda.Shutdown();
    });
}
