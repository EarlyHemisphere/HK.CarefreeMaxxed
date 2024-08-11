using System.CodeDom;
using Modding;

namespace CarefreeMaxxed {
    public class CarefreeMaxxed : Mod {
        public static CarefreeMaxxed instance;
        private bool activateCFM = true;

        public CarefreeMaxxed() : base("Carefree Maxxed") => instance = this;

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public bool ToggleButtonInsideMenu => false;

        public override void Initialize() {
            Log("Initializing");

            ModHooks.TakeDamageHook += TakeDamage;
            ModHooks.AfterTakeDamageHook += AfterTakeDamage;

            Log("Initialized");
        }

        public int TakeDamage(ref int _, int damageAmount) {
            if (activateCFM) {
                HeroController instance = HeroController.instance;
                if (instance != null) {
                    ReflectionHelper.SetField(instance, "hitsSinceShielded", 7);
                }
            }
            return damageAmount;
        }

        public int AfterTakeDamage(int _, int damageAmount) {
            if (activateCFM) {
                HeroController instance = HeroController.instance;
                if (instance != null) {
                    ReflectionHelper.SetField(instance, "hitsSinceShielded", 0);
                }
            }
            activateCFM = !activateCFM;
            return damageAmount;
        }
    }
}
