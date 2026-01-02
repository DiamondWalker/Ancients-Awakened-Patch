using AAMod.Globals.Players;
using AAMod.Globals.Worlds;
using AAMod.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace AAMod.NPCs.Bosses.MushroomMonarch {
    [AutoloadBossHead]
    public class MushroomMonarch : ModNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom Monarch");
            Main.npcFrameCount[npc.type] = 18;
        }

        public override void SetDefaults()
        {
            npc.lifeMax = 1200;   //boss life
            npc.damage = 24;  //boss damage
            npc.defense = 12;    //boss defense
            npc.knockBackResist = 0f;   //this boss will behavior like the DemonEye  //boss frame/animation 
            npc.value = Item.sellPrice(0, 0, 50, 0);
            npc.aiStyle = -1;
            npc.width = 78;
            npc.height = 108;
            npc.npcSlots = 1f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.buffImmune[46] = true;
            npc.buffImmune[47] = true;
            npc.netAlways = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            bossBag = mod.ItemType("MonarchBag");
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Monarch");

        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public const int AISTATE_DEFAULT = 0, AISTATE_CHARGE = 1, AISTATE_JUMP = 2, AISTATE_SPAWN = 3, AISTATE_FLY = 4;
        private int State { get => (int)npc.ai[0]; set => npc.ai[0] = value; }
        private int TimeInState { get => (int)npc.ai[1]; set => npc.ai[1] = value; }
        private float AttackParameterF { get => npc.ai[2]; set => npc.ai[2] = value; }
        private int AttackParameterI { get => (int)npc.ai[2]; set => npc.ai[2] = value; }
        private float AttackParameter2F { get => npc.ai[3]; set => npc.ai[3] = value; }
        private int AttackParameter2I { get => (int)npc.ai[3]; set => npc.ai[3] = value; }
        private Vector2 FlyTo { 
            get { 
                return new Vector2(npc.ai[2], npc.ai[3]); 
            } set {
                npc.ai[2] = value.X;
                npc.ai[3] = value.Y;
            } 
        }

        private bool longJump = Main.expertMode;
        private int despawnTimer = 0;

        private int stuckTime = 0;

        private static readonly int[] ANIM_IDLE = new int[] { 0, 1 };
        private static readonly int[] ANIM_WALK = new int[] { 2, 3, 4, 5 };
        private static readonly int[] ANIM_JUMP = new int[] { 6, 7, 8 };
        private static readonly int[] ANIM_SPAWN = new int[] { 9, 10, 11, 12 };
        private const int SPAWN_FRAME = 13;
        private static readonly int[] ANIM_FLY = new int[] { 14, 15, 16, 17 };
		
        public override void AI()
        {
            //targetting/despawning
            npc.TargetClosest();

            Player player = Main.player[npc.target];

            bool despawn = false;
            if (player == null)
            {
                npc.TargetClosest();
            }

            if (player.dead || !player.active || Vector2.Distance(player.Center, npc.Center) > 5000)
            {
                npc.TargetClosest();

                if (player.dead || !player.active || Vector2.Distance(player.Center, npc.Center) > 5000)
                {
                    despawn = true;
                }
            }

            if (Main.netMode != 1 && !despawn && !player.GetModPlayer<AAPlayer>().ZoneMush) {
                if (despawnTimer++ > 300) {
                    despawn = true;
                } 
            } else {
                despawnTimer = 0;
            }

            if (despawn) {
                Projectile.NewProjectile(npc.Center, new Vector2(0f, 0f), mod.ProjectileType("MonarchRUNAWAY"), 0, 0);
                npc.active = false;
                return;
            }


            // animations
            if (State == AISTATE_SPAWN && TimeInState <= 90 + 30) { // spawning mushlings
                if (TimeInState >= 90) {
                    AnimationHelper.SetFrame(npc, SPAWN_FRAME);
                } else {
                    AnimationHelper.UpdateAnimation(npc, ANIM_SPAWN, 4);
                }
            } else if (State != AISTATE_JUMP && State != AISTATE_FLY) //walk or charge
            {
                if (npc.velocity.X != 0) {
                    AnimationHelper.UpdateAnimation(npc, ANIM_WALK, 15, (int)Math.Ceiling(Math.Abs(npc.velocity.X)));
                }

                if (npc.velocity.Y != 0 || npc.velocity.X == 0) {
                    if (npc.velocity.Y < 0) {
                        AnimationHelper.SetFrame(npc, ANIM_JUMP[1]);
                    } else if (npc.velocity.Y > 0) {
                        AnimationHelper.SetFrame(npc, ANIM_JUMP[2]);
                    } else {
                        AnimationHelper.UpdateAnimation(npc, ANIM_IDLE, 15);
                    }
                }
            } else if (State == AISTATE_FLY) { // flying
                AnimationHelper.UpdateAnimation(npc, ANIM_FLY, 4);

            } else //jump
              {
                int jumpFrame;
                if (npc.velocity.Y > 0) {
                    jumpFrame = 2;
                } else if (npc.velocity.Y < 0) {
                    jumpFrame = 1;
                } else {
                    jumpFrame = 0;
                }
                AnimationHelper.SetFrame(npc, ANIM_JUMP[jumpFrame]);
            }
            
            if (State != AISTATE_CHARGE && State != AISTATE_JUMP) {
                if (npc.velocity.X > 0) {
                    npc.spriteDirection = -1;
                } else if (npc.velocity.X < 0) {
                    npc.spriteDirection = 1;
                }
            }

            // for the afterimage
            for (int m = npc.oldPos.Length - 1; m > 0; m--) {
                npc.oldPos[m] = npc.oldPos[m - 1];
            }
            npc.oldPos[0] = (State == AISTATE_CHARGE) ? npc.position : Vector2.Zero;


            // AI
            switch (State) {
                case AISTATE_DEFAULT:
                    if (flyIfOutOfRange(player)) break; // if the player is above or below, enter flying mode

                    float move = player.Center.X - npc.Center.X;
                    move /= Math.Abs(move);
                    move *= 3.0f;
                    if (!Walk(0.17f, move, true, 3, 4, true, null, false)) {
                        stuckTime++;
                        if (stuckTime > 30) { // if I am stuck, enter flying mode
                            flyTo(player);
                        }
                    }

                    // use special attack
                    if (TimeInState > 120 && Main.rand.Next(200) == 0) {
                        ChangeState(Main.rand.Next(1, 4));
                        npc.velocity.X = 0;
                    }

                    break;

                case AISTATE_CHARGE:
                    // we've gone past the player (or the attack has gone on too long). Return to default behavior
                    if (TimeInState > 300 || (npc.spriteDirection == -1 && npc.Center.X > player.Center.X + 300) || (npc.spriteDirection == 1 && npc.Center.X < player.Center.X - 300)) {
                        ChangeState(0);
                    } else if (TimeInState > (Main.expertMode ? 10 : 30)) {
                        if (!Walk(Main.expertMode ? 0.14f : 0.07f, 10 * -npc.spriteDirection, false, 3, 4, false, null, true)) { // we hit a wall. Return to default behavior
                            ChangeState(0);
                            // TODO: screen shake
                        }
                    } else {
                        // we are facing the player to prepare for the charge
                        if (npc.Center.X < player.Center.X) {
                            npc.spriteDirection = -1;
                        } else {
                            npc.spriteDirection = 1;
                        }
                    }

                    break;

                case AISTATE_JUMP: {
                      if (npc.velocity.Y == 0) { // we are on the ground so we run the jumping AI
                            if (flyIfOutOfRange(player)) break; // before every jump, we want to check if player is in range, otherwise enter flying mode

                            // if the previous jump put us on the other side of the player (i.e. we caught up to them), our next jump will be smaller and more precise (so we don't just jump over them again)
                            int newDirection;
                            if (npc.Center.X < player.Center.X) {
                                newDirection = -1;
                            } else {
                                newDirection = 1;
                            }
                            if (newDirection != npc.spriteDirection) {
                                npc.spriteDirection = newDirection;
                                if (AttackParameter2I > 0) longJump = false; // we only want to do short jumps AFTER our first attack. Otherwise it'll be easy to dodge since player can just walk away
                            }

                            // we've done 6 jumps so we switch to default behavior
                            if (AttackParameter2I >= 6) {
                                ChangeState(0);
                                longJump = Main.expertMode; // we want to set this flag so next time we do the attack we'll start with a long jump again
                                break;
                            }

                            npc.velocity.X = 0; // let's make sure we aren't walking while preparing to jump

                            int jumpChargeupTime;
                            float speed;
                            if (longJump) {
                                // for long jumps, we move a fixed distance. We do this to try and cut off the player if they're running away from us
                                jumpChargeupTime = AttackParameter2I == 0 ? 15 : 5;
                                speed = 5;
                            } else {
                                // for short jumps, we will try to land directly on the player
                                jumpChargeupTime = Main.expertMode ? 10 : 40;
                                speed = Math.Abs(player.Center.X -  npc.Center.X);
                                speed = Math.Min(speed / 53, 8); // 53 ticks is the airtime for a jump on flat ground
                            }
                            // now actually perform the jump based on chosen parameters
                            AttackParameterI++;
                            if (AttackParameterI >= jumpChargeupTime) {
                                npc.velocity.Y = -8;
                                npc.velocity.X = -speed * npc.spriteDirection;
                                npc.noTileCollide = true; // we do this so our jump doesn't get stopped by any obstacles in our way
                                AttackParameterI = 0;
                                AttackParameter2I++;
                                if (Main.expertMode) longJump = true;
                            }
                        }

                        if (npc.velocity.Y >= 0) npc.noTileCollide = false; // we are falling and want to land on the ground, so lets turn tile collide back on

                        break;
                    }
                case AISTATE_SPAWN: {
                        npc.velocity.X = 0; // we don't want to move while we're spawning minions

                        if (AttackParameterI == 0) {
                            if (npc.velocity.Y == 0 && TimeInState >= 90) {
                                AttackParameterI = 1;
                                int Minion1 = NPC.NewNPC((int)npc.Center.X + 20, (int)npc.Center.Y, ModContent.NPCType<RedMushling>(), 0);
                                int Minion2 = NPC.NewNPC((int)npc.Center.X - 20, (int)npc.Center.Y, ModContent.NPCType<RedMushling>(), 0);
                                Main.npc[Minion1].netUpdate = true;
                                Main.npc[Minion2].netUpdate = true;
                            }
                        } else {
                            if (TimeInState >= 130) {
                                ChangeState(0);
                            }
                        }

                        break;
                    }
                case AISTATE_FLY: {
                        Vector2 desiredMovement = (FlyTo - npc.Bottom) / 5; // the desired velocity, scaled with distance to the player

                        // accelerate such that our velocity approaches the desired
                        Vector2 acceleration = desiredMovement - npc.velocity;
                        acceleration.Normalize();

                        // apply the acceleration but limit our velocity
                        npc.velocity += acceleration;
                        if (npc.velocity.Length() > 15) {
                            npc.velocity.Normalize();
                            npc.velocity *= 15;
                        }
                        npc.rotation = npc.velocity.X / 50;

                        // have we reached our target? If so, change back to standard melee AI
                        if (Vector2.Distance(FlyTo, npc.Bottom) < 20) ChangeState(0);

                        break;
                    }
            }
            TimeInState++;

            return;
        }

        private bool flyIfOutOfRange(Player player) {
            if (npc.velocity.Y == 0 && player.velocity.Y == 0 && Math.Abs(player.Center.Y - npc.Center.Y) > player.height + npc.height) {
                flyTo(player);
                return true;
            }
            return false;
        }

        private void flyTo(Player player) {
            ChangeState(AISTATE_FLY);
            FlyTo = player.Center;
        }

        private void ChangeState(int newState) {
            if (State != AISTATE_FLY && newState != AISTATE_FLY) { // we don't want to reset TimeInState when we enter flying mode. Otherwise, players can repeatedly trigger flying mode and prevent us from ever using other attacks
                TimeInState = 0;
            }
            if (State == AISTATE_FLY) {
                // flying mode change some parameters ofc, so if we were in a flying mode we want to set everything back to normal
                if (Main.expertMode) npc.damage = npc.defDamage;
                npc.noTileCollide = false;
                npc.noGravity = false;
                npc.rotation = 0;
            } else if (newState == AISTATE_FLY) {
                if (Main.expertMode) npc.damage *= 2; // flying mode will double our contact damage in expert mode
                npc.noTileCollide = true;
                npc.noGravity = true;
            }
            
            State = newState;
            AttackParameterF = 0;
            AttackParameter2F = 0;
            npc.netUpdate = true;
        }

        private bool Walk(float acceleration, float desiredVelocity, bool jump, int maxJumpTilesX = 3, int maxJumpTilesY = 4, bool jumpUpPlatforms = false, Action<bool, bool, Vector2, Vector2> onTileCollide = null, bool ignoreJumpTiles = false) {
            npc.TargetClosest(true);

            bool wasStopped = npc.velocity.X == 0;
            if (acceleration < 0) throw new ArgumentOutOfRangeException("Acceleration is " + acceleration + ". It should not be negative!");
            if (acceleration == float.MaxValue) {
                npc.velocity.X = desiredVelocity;
            } else {
                if (npc.velocity.X > desiredVelocity) {
                    npc.velocity.X -= acceleration;
                    if (npc.velocity.X < desiredVelocity) npc.velocity.X = desiredVelocity;
                }
                if (npc.velocity.X < desiredVelocity) {
                    npc.velocity.X += acceleration;
                    if (npc.velocity.X > desiredVelocity) npc.velocity.X = desiredVelocity;
                }
            }
            BaseAI.WalkupHalfBricks(npc);
            
            if (BaseAI.HitTileOnSide(npc, 3)) {
                //if the npc's velocity is going in the same direction as the npc's direction...
                if ((npc.velocity.X < 0f && npc.direction == -1) || (npc.velocity.X > 0f && npc.direction == 1)) {
                    //...attempt to jump if needed.
                    Vector2 newVec = jump ? BaseAI.AttemptJump(npc.position, npc.velocity, npc.width, npc.height, npc.direction, npc.directionY, maxJumpTilesX, maxJumpTilesY, Math.Abs(npc.velocity.X), jumpUpPlatforms, npc.HasValidTarget ? Main.player[npc.target] : null, ignoreJumpTiles) : npc.velocity;
                    if (!npc.noTileCollide) {
                        newVec = Collision.TileCollision(npc.position, newVec, npc.width, npc.height);
                        Vector4 slopeVec = Collision.SlopeCollision(npc.position, newVec, npc.width, npc.height);
                        Vector2 slopeVel = new Vector2(slopeVec.Z, slopeVec.W);
                        if (onTileCollide != null && npc.velocity != slopeVel) onTileCollide(npc.velocity.X != slopeVel.X, npc.velocity.Y != slopeVel.Y, npc.velocity, slopeVel);
                        npc.position = new Vector2(slopeVec.X, slopeVec.Y);
                        npc.velocity = slopeVel;
                    }
                    if (npc.velocity != newVec) { npc.velocity = newVec; npc.netUpdate = true; }
                }
            }

            if (wasStopped && desiredVelocity != 0 && npc.velocity.X == 0) return false;
            return true;
        }
        
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.LesserHealingPotion;
            AAWorld.downedMonarch = true;
            Projectile.NewProjectile(npc.Center, new Vector2(0f, 0f), mod.ProjectileType("MonarchRUNAWAY"), 0, 0);
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SporeSac"), Main.rand.Next(30, 35));
            if (Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MonarchTrophy"));
            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MonarchMask"));
                }

                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Mushium"), Main.rand.Next(25, 35));
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor) {
            Texture2D bodyTex = Main.npcTexture[npc.type];
            Color lightColor = BaseDrawing.GetNPCColor(npc, null);
            Color trailColor = lightColor;
            trailColor.R = (byte)(0.3 * trailColor.R);
            trailColor.G = (byte)(0.3 * trailColor.G);
            trailColor.B = (byte)(0.3 * trailColor.B);
            trailColor.A = (byte)(0.3 * trailColor.A);
            BaseDrawing.DrawAfterimage(sb, bodyTex, 0, npc, 1f, 1.0f, 10, true, 0f, 0f, trailColor/*Color.DarkRed*/);
            BaseDrawing.DrawTexture(sb, bodyTex, 0, npc, lightColor);
            return false;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);  //boss life scale in expertmode
            npc.damage = (int)(npc.damage * 0.6f);
        }
    }
}


