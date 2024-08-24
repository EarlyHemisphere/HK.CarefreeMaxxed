using Modding;
using SFCore;
using SFCore.Utils;

namespace CarefreeMaxxed {
    public class CarefreeMaxxed : Mod {
        public static CarefreeMaxxed instance;
        private bool activateCFM = true;
        private HeroController heroController;
        private PlayMakerFSM damageFSM;

        public CarefreeMaxxed() : base("Carefree Maxxed") => instance = this;

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public bool ToggleButtonInsideMenu => false;

        public override void Initialize() {
            Log("Initializing");

            ModHooks.AfterTakeDamageHook += AfterTakeDamage;
            On.HeroController.Awake += HeroControllerAwake;
            On.PlayMakerFSM.OnEnable += OnEnable;

            Log("Initialized");
        }

        public void HeroControllerAwake(On.HeroController.orig_Awake orig, HeroController self) {
            orig(self);

            heroController = self;
        }

        public void OnEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
            orig(self);

            if (self.FsmName == "Knight Damage") {
                damageFSM = self;
            }
        }

        public int AfterTakeDamage(int _, int damageAmount) {
            int damage = damageAmount;

            if (activateCFM) {
                damage = 0;
                heroController.carefreeShield.SetActive(true);
                damageFSM.RemoveFsmTransition("Idle", "DAMAGE");
            } else {
                ReflectionHelper.SetField(heroController, "hitsSinceShielded", 0);
                damageFSM.AddFsmTransition("Idle", "DAMAGE", "Muffle?");
            }

            activateCFM = !activateCFM;

            return damage;
        }
    }
}
