using Avalonia.Platform.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClipboardSpy.ViewModels;

public class FormatViewModel : ViewModelBase {
    public string FormatDataDisplayValue { get; private set; }
    public string FormatName { get; set; }
    public string FormatType =>
        FormatData == null ? "NULL" : FormatData.GetType().ToString();

    private object _formatData;
    public object FormatData {
        get => _formatData;
        set {
            _formatData = value;
            this.RaisePropertyChanged(nameof(FormatData));
            switch (_formatData) {
                case IEnumerable<IStorageItem> sil:
                    FormatDataDisplayValue = string.Join(Environment.NewLine, sil.Select(x => x.TryGetLocalPath()));
                    break;
                case byte[] bytes:
                    if (FormatName == "PNG") {
                        FormatDataDisplayValue = Convert.ToBase64String(bytes);
                    } else {
                        FormatDataDisplayValue = Encoding.Default.GetString(bytes);
                    }
                    break;
                default:
                    FormatDataDisplayValue = _formatData == null ? string.Empty : _formatData.ToString();
                    break;
            }
        }
    }

    public FormatViewModel() { }
}
