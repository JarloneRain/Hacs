using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hacs;

abstract class KeyConfigForm : Form
{
    protected const int DefaultWidth = 600;
    protected const int DefaultHeight = 1000;
    protected const int LabelHeight = 50;
    protected const int TextBoxHeight = 100;
    protected const int TextBoxMargin = 50;
    protected const int BothHeight = 200;
    protected SplitContainer splitContainer = new()
    {
        Dock = DockStyle.Fill,
        Orientation = Orientation.Horizontal
    };

    protected Panel InfoPannel => splitContainer.Panel1;
    protected Panel ButtonPannel => splitContainer.Panel2;

    // Keys
    protected readonly Label keysLabel = new()
    { Text = "Keys", Location = new Point(10, 0 * BothHeight + 20), Height = LabelHeight, Width = 200 };
    protected readonly TextBox keysTextBox = new()
    { Location = new Point(TextBoxMargin, 0 * BothHeight + 90), Height = TextBoxHeight, Width = DefaultWidth - 2 * TextBoxMargin };
    // OffsetX
    protected readonly Label offsetXLabel = new()
    { Text = "OffsetX", Location = new Point(10, 1 * BothHeight + 20), Height = LabelHeight, Width = 200 };
    protected readonly TextBox offsetXTextBox = new()
    { Location = new Point(TextBoxMargin, 1 * BothHeight + 90), Height = TextBoxHeight, Width = DefaultWidth - 2 * TextBoxMargin };
    // OffsetY
    protected readonly Label offsetYLabel = new()
    { Text = "OffsetY", Location = new Point(10, 2 * BothHeight + 20), Height = LabelHeight, Width = 200 };
    protected readonly TextBox offsetYTextBox = new()
    { Location = new Point(TextBoxMargin, 2 * BothHeight + 90), Height = TextBoxHeight, Width = DefaultWidth - 2 * TextBoxMargin };
    // Description
    protected readonly Label descriptionLabel = new()
    { Text = "Description", Location = new Point(10, 3 * BothHeight + 20), Height = LabelHeight, Width = 200 };
    protected readonly TextBox descriptionTextBox = new()
    { Location = new Point(TextBoxMargin, 3 * BothHeight + 90), Height = TextBoxHeight, Width = DefaultWidth - 2 * TextBoxMargin };

    public KeyConfigForm(KeyConfig keyConfig)
    {
        Icon = Resource.hacs;

        Controls.Add(splitContainer);

        InfoPannel.Controls.Add(keysLabel);
        InfoPannel.Controls.Add(keysTextBox);
        InfoPannel.Controls.Add(offsetXLabel);
        InfoPannel.Controls.Add(offsetXTextBox);
        InfoPannel.Controls.Add(offsetYLabel);
        InfoPannel.Controls.Add(offsetYTextBox);
        InfoPannel.Controls.Add(descriptionLabel);
        InfoPannel.Controls.Add(descriptionTextBox);

        keysTextBox.Text = keyConfig.Keys;
        offsetXTextBox.Text = keyConfig.OffsetX.ToString();
        offsetYTextBox.Text = keyConfig.OffsetY.ToString();
        descriptionTextBox.Text = keyConfig.Description;

        ClientSize = new(DefaultWidth, DefaultHeight);
        splitContainer.SplitterDistance = 800;

        Resize += (_, _) =>
        {
            splitContainer.SplitterDistance = ClientSize.Height - 200;

            keysTextBox.Width = splitContainer.Width - 2 * TextBoxMargin;
            offsetXTextBox.Width = splitContainer.Width - 2 * TextBoxMargin;
            offsetYTextBox.Width = splitContainer.Width - 2 * TextBoxMargin;
            descriptionTextBox.Width = splitContainer.Width - 2 * TextBoxMargin;
        };
    }
}

class ModifyKeyConfigForm : KeyConfigForm
{
    readonly Button modifyBtn = new()
    {
        Text = "Modify",
        Width = 160,
        Height = 100,
        Location = new(1 * DefaultWidth / 3 - 80, 20)
    };
    readonly Button cancelBtn = new()
    {
        Text = "Cancel",
        Width = 160,
        Height = 100,
        Location = new(2 * DefaultWidth / 3 - 80, 20)
    };
    public ModifyKeyConfigForm(KeyConfig keyConfig, Action<KeyConfig> onModify)
        : base(keyConfig)
    {
        ButtonPannel.Controls.Add(modifyBtn);
        ButtonPannel.Controls.Add(cancelBtn);

        modifyBtn.Click += (_, _) =>
        {
            try
            {
                onModify(new KeyConfig
                {
                    Keys = keysTextBox.Text,
                    OffsetX = int.Parse(offsetXTextBox.Text),
                    OffsetY = int.Parse(offsetYTextBox.Text),
                    Description = descriptionTextBox.Text
                });
            }
            catch(Exception e)
            {
                MessageBox.Show($"{e.Message}", "Error");
            }
            Close();
        };
        cancelBtn.Click += (_, _) => Close();

        Resize += (_, _) =>
        {
            modifyBtn.Location = modifyBtn.Location with { X = 1 * modifyBtn.Parent!.Width / 3 - modifyBtn.Width / 2 };
            cancelBtn.Location = cancelBtn.Location with { X = 2 * cancelBtn.Parent!.Width / 3 - cancelBtn.Width / 2 };
        };
    }
}

class AddKeyConfigForm : KeyConfigForm
{
    readonly Button addBtn = new()
    {
        Text = "Add",
        Width = 160,
        Height = 100,
        Location = new(1 * DefaultWidth / 3 - 80, 20)
    };
    readonly Button cancelBtn = new()
    {
        Text = "Cancel",
        Width = 160,
        Height = 100,
        Location = new(2 * DefaultWidth / 3 - 80, 20)
    };
    public AddKeyConfigForm(Action<KeyConfig> onAdd)
        : base(new KeyConfig())
    {
        ButtonPannel.Controls.Add(addBtn);
        ButtonPannel.Controls.Add(cancelBtn);

        addBtn.Click += (_, _) =>
        {
            try
            {
                onAdd(new KeyConfig
                {
                    Keys = keysTextBox.Text,
                    OffsetX = int.Parse(offsetXTextBox.Text),
                    OffsetY = int.Parse(offsetYTextBox.Text),
                    Description = descriptionTextBox.Text
                });
            }
            catch(Exception e)
            {
                MessageBox.Show($"{e.Message}", "Error");
            }

            Close();
        };
        cancelBtn.Click += (_, _) => Close();

        Resize += (_, _) =>
        {
            addBtn.Location = addBtn.Location with { X = 1 * addBtn.Parent!.Width / 3 - addBtn.Width / 2 };
            cancelBtn.Location = cancelBtn.Location with { X = 2 * cancelBtn.Parent!.Width / 3 - cancelBtn.Width / 2 };
        };
    }
}