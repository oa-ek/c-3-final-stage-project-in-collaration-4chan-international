using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Models_Context.Models;
using Services.DataTransferObj;
using Services.Interfaces;
using Program.Utilities;

namespace Program.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ICatalogService _catalogService;
        private readonly IBuildCalculatorService _calcService;
        private readonly IValidationService _validationService;
        private readonly ICharacterBuildService _buildService;

       
        private int _currentBuildId = 0;

        private string _buildName = "New Build";
        public string BuildName { get => _buildName; set { _buildName = value; OnPropertyChanged(); } }

        public ICommand OpenSlotCommand { get; }
        public ICommand SelectItemCommand { get; }
        public ICommand CloseInventoryCommand { get; }
        public ICommand UnequipCommand { get; }
        public ICommand SaveAndExitCommand { get; }
        public Action CloseAction { get; set; }

        public MainViewModel(ICatalogService catalogService,
                             IBuildCalculatorService calcService,
                             IValidationService validationService,
                             ICharacterBuildService buildService)
        {
            _catalogService = catalogService;
            _calcService = calcService;
            _validationService = validationService;
            _buildService = buildService;

            OpenSlotCommand = new RelayCommand(OpenSlot);
            SelectItemCommand = new RelayCommand(SelectItem);
            CloseInventoryCommand = new RelayCommand(obj => IsInventoryOpen = false);
            UnequipCommand = new RelayCommand(UnequipItem);
            SaveAndExitCommand = new RelayCommand(SaveAndExit);

            Recalculate();
        }

        // --- ЗАВАНТАЖЕННЯ (З меню в редактор) ---
        public void LoadBuild(CharacterBuild build)
        {
            if (build == null) return;

            _currentBuildId = build.Id;
            BuildName = build.Name;

           
            Vigor = build.Vigor ?? 10;
            Endurance = build.Endurance ?? 10;
            Strength = build.Strength ?? 10; 
            Dexterity = build.Dexterity ?? 10;
            Intelligence = build.Intelligence ?? 10;
            Faith = build.Faith ?? 10;

            // шукаємо предмети їх в каталозі по ID
            if (build.RightHandId.HasValue) SelectedRightHand = _catalogService.GetAllWeapons().FirstOrDefault(w => w.WeaponId == build.RightHandId);
            if (build.LeftHandId.HasValue) SelectedLeftHand = _catalogService.GetAllWeapons().FirstOrDefault(w => w.WeaponId == build.LeftHandId);

            if (build.HeadId.HasValue) SelectedHead = _catalogService.GetArmorBySlot(1).OfType<Armor>().FirstOrDefault(a => a.ArmorId == build.HeadId);
            if (build.ChestId.HasValue) SelectedChest = _catalogService.GetArmorBySlot(2).OfType<Armor>().FirstOrDefault(a => a.ArmorId == build.ChestId);
            if (build.HandsId.HasValue) SelectedHands = _catalogService.GetArmorBySlot(3).OfType<Armor>().FirstOrDefault(a => a.ArmorId == build.HandsId);
            if (build.LegsId.HasValue) SelectedLegs = _catalogService.GetArmorBySlot(4).OfType<Armor>().FirstOrDefault(a => a.ArmorId == build.LegsId);

            Recalculate();
        }

    
        private void SaveAndExit(object obj)
        {
            try
            {
                var build = new CharacterBuild
                {
                    Id = _currentBuildId, 
                    Name = BuildName,

                    Vigor = Vigor,
                    Endurance = Endurance,
                    Strength = Strength,
                    Dexterity = Dexterity,
                    Intelligence = Intelligence,
                    Faith = Faith,

                    RightHandId = SelectedRightHand?.WeaponId,
                    LeftHandId = SelectedLeftHand?.WeaponId,
                    HeadId = SelectedHead?.ArmorId,
                    ChestId = SelectedChest?.ArmorId,
                    HandsId = SelectedHands?.ArmorId,
                    LegsId = SelectedLegs?.ArmorId
                };

                _buildService.SaveBuild(build);

                MessageBox.Show("Saved successfully!");

                
                CloseAction?.Invoke();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Error saving: {ex.Message}");
            }
        }

      
        public int RWeaponCriticalDmg
        {
            // Вона повинна просто повертати значення зі Stats
            get { return Stats != null ? Stats.RWeaponCritical : 0; }
        }

        private bool _isTwoHanding;
        public bool IsTwoHanding
        {
            get => _isTwoHanding;
            set
            {
                _isTwoHanding = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EffectiveStrengthText));
                Recalculate();
            }
        }

        private Weapon _selectedLeftHand;
        public Weapon SelectedLeftHand
        {
            get => _selectedLeftHand;
            set
            {
                _selectedLeftHand = value;
                
                if (_selectedLeftHand != null) IsTwoHanding = false;
                OnPropertyChanged();
                Recalculate();
            }
        }

        
        private int _vigor = 12; public int Vigor { get => _vigor; set { _vigor = value; OnPropertyChanged(); Recalculate(); } }
        private int _mind = 10; public int Mind { get => _mind; set { _mind = value; OnPropertyChanged(); Recalculate(); } }
        private int _endurance = 11; public int Endurance { get => _endurance; set { _endurance = value; OnPropertyChanged(); Recalculate(); } }
        private int _strength = 14; public int Strength { get => _strength; set { _strength = value; OnPropertyChanged(); OnPropertyChanged(nameof(EffectiveStrengthText)); Recalculate(); } }
        private int _dexterity = 14; public int Dexterity { get => _dexterity; set { _dexterity = value; OnPropertyChanged(); Recalculate(); } }
        private int _intelligence = 10; public int Intelligence { get => _intelligence; set { _intelligence = value; OnPropertyChanged(); Recalculate(); } }
        private int _faith = 10; public int Faith { get => _faith; set { _faith = value; OnPropertyChanged(); Recalculate(); } }
        public string EffectiveStrengthText => IsTwoHanding ? $"({(int)(Strength * 1.5)})" : "";
        private string _validationMessage; public string ValidationMessage { get => _validationMessage; set { _validationMessage = value; OnPropertyChanged(); } }

        private Weapon _selectedRightHand; public Weapon SelectedRightHand { get => _selectedRightHand; set { _selectedRightHand = value; OnPropertyChanged(); Recalculate(); } }
       
        private Armor _selectedHead; public Armor SelectedHead { get => _selectedHead; set { _selectedHead = value; OnPropertyChanged(); Recalculate(); } }
        private Armor _selectedChest; public Armor SelectedChest { get => _selectedChest; set { _selectedChest = value; OnPropertyChanged(); Recalculate(); } }
        private Armor _selectedHands; public Armor SelectedHands { get => _selectedHands; set { _selectedHands = value; OnPropertyChanged(); Recalculate(); } }
        private Armor _selectedLegs; public Armor SelectedLegs { get => _selectedLegs; set { _selectedLegs = value; OnPropertyChanged(); Recalculate(); } }

        private BuildStats _stats; public BuildStats Stats { get => _stats; set { _stats = value; OnPropertyChanged(); } }

        public string RWeaponPhysicalDmg => GetWeaponDamage(SelectedRightHand, "Physical");
        public string RWeaponMagicDmg => GetWeaponDamage(SelectedRightHand, "Magic");
        public string RWeaponFireDmg => GetWeaponDamage(SelectedRightHand, "Fire");
        public string RWeaponLightningDmg => GetWeaponDamage(SelectedRightHand, "Lightning");
        

        private string GetWeaponDamage(Weapon weapon, string type)
        {
            if (weapon == null) return "-";
            var influence = weapon.WeaponInfluences.FirstOrDefault(i => i.InfluenceType.Name == type);
            if (influence == null) return "-";

            double? dmg = influence.Value;
            
            if (IsTwoHanding && weapon == SelectedRightHand) dmg *= 1.3;

            return $"{dmg:N0}";
        }

        
        private bool _isInventoryOpen;
        public bool IsInventoryOpen { get => _isInventoryOpen; set { _isInventoryOpen = value; OnPropertyChanged(); } }
        public ObservableCollection<object> InventoryItems { get; set; } = new ObservableCollection<object>();
        private string _currentSlotType;

        private void UnequipItem(object obj)
        {
            switch (_currentSlotType)
            {
                case "Head": SelectedHead = null; break;
                case "Chest": SelectedChest = null; break;
                case "Hands": SelectedHands = null; break;
                case "Legs": SelectedLegs = null; break;
                case "RightHand": SelectedRightHand = null; break;
                case "LeftHand": SelectedLeftHand = null; break;
            }
            IsInventoryOpen = false;
        }

        private void OpenSlot(object slotType)
        {
            _currentSlotType = slotType.ToString();
            InventoryItems.Clear();
            IEnumerable<object> items = null;
            try
            {
                switch (_currentSlotType)
                {
                    case "Head": items = _catalogService.GetArmorBySlot(1); break;
                    case "Chest": items = _catalogService.GetArmorBySlot(2); break;
                    case "Hands": items = _catalogService.GetArmorBySlot(3); break;
                    case "Legs": items = _catalogService.GetArmorBySlot(4); break;
                    case "RightHand":
                    case "LeftHand": items = _catalogService.GetAllWeapons(); break;
                }
                if (items != null) foreach (var item in items) InventoryItems.Add(item);
                IsInventoryOpen = true;
            }
            catch (System.Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void SelectItem(object item)
        {
            if (item is Armor armor)
            {
                if (_currentSlotType == "Head") SelectedHead = armor;
                else if (_currentSlotType == "Chest") SelectedChest = armor;
                else if (_currentSlotType == "Hands") SelectedHands = armor;
                else if (_currentSlotType == "Legs") SelectedLegs = armor;
            }
            else if (item is Weapon weapon)
            {
                if (_currentSlotType == "RightHand") SelectedRightHand = weapon;
                else if (_currentSlotType == "LeftHand") SelectedLeftHand = weapon;
            }
            IsInventoryOpen = false;
        }

        private void Recalculate()
        {
            try
            {
                var weaponIds = new List<int>();
                if (SelectedRightHand != null) weaponIds.Add(SelectedRightHand.WeaponId);
                if (SelectedLeftHand != null) weaponIds.Add(SelectedLeftHand.WeaponId);
                var armorIds = new List<int>();
                if (SelectedHead != null) armorIds.Add(SelectedHead.ArmorId);
                if (SelectedChest != null) armorIds.Add(SelectedChest.ArmorId);
                if (SelectedHands != null) armorIds.Add(SelectedHands.ArmorId);
                if (SelectedLegs != null) armorIds.Add(SelectedLegs.ArmorId);

                int effectiveStr = IsTwoHanding ? (int)(Strength * 1.5) : Strength;
                var errors = _validationService.ValidateEquipmentRequirements(effectiveStr, Dexterity, Intelligence, Faith, weaponIds);
                ValidationMessage = errors.Any() ? "!!!!!" + string.Join("\n!!!!!! ", errors) : "";

                Stats = _calcService.CalculateStats(Vigor, Endurance, Mind, weaponIds, armorIds);

                OnPropertyChanged(nameof(RWeaponPhysicalDmg));
                OnPropertyChanged(nameof(RWeaponMagicDmg));
                OnPropertyChanged(nameof(RWeaponFireDmg));
                OnPropertyChanged(nameof(RWeaponLightningDmg));
                OnPropertyChanged(nameof(RWeaponCriticalDmg));
                OnPropertyChanged(nameof(RWeaponCriticalDmg));
            }
            catch { }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}