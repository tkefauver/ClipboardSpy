using Avalonia.Controls;
using Avalonia.Input;
using ClipboardSpy.ViewModels;

namespace ClipboardSpy.Views;

public partial class MainView : UserControl {
    public MainView() {
        InitializeComponent();
        lb.AddHandler(DragDrop.DropEvent, Drop);
    }
    void Drop(object sender, DragEventArgs e) {
        var dc = DataContext as MainViewModel;
        dc.SetDataObject(e.Data);
    }
}
