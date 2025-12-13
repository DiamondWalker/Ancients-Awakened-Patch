using System;
using System.IO;
using AAMod.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace AAMod.NPCs.Bosses.MushroomMonarch
{
    [AutoloadBossHead]
    public class MushroomMonarch : ModNPC
    {
		/*public override void SendExtraAI(BinaryWriter writer)
		{
			base.SendExtraAI(writer);
			if(Main.netMode == NetmodeID.Server || Main.dedServ)
			{
				writer.Write(internalAI[0]);
				writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
            }
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			base.ReceiveExtraAI(reader);
			if(Main.netMode == NetmodeID.MultiplayerClient)
			{
				internalAI[0] = reader.ReadFloat();
				internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
            }	
		}*/

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

        private int flyFrame = 0;


        private readonly int[] ANIM_IDLE = new int[] { 0, 1 };
        private readonly int[] ANIM_WALK = new int[] { 2, 3, 4, 5 };
        private readonly int[] ANIM_JUMP = new int[] { 6, 7, 8 };
        private readonly int[] ANIM_SPAWN = new int[] { 9, 10, 11, 12 };
        private readonly int SPAWN_FRAME = 13;
        private readonly int[] ANIM_FLY = new int[] { 14, 15, 16, 17 };
		
        public override void AI()
        {
            // targetting/despawn
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

            //float dist = npc.Distance(player.Center);

            // animation
            if (State == AISTATE_SPAWN && TimeInState <= 90 + 30) {
                if (TimeInState >= 90) {
                    npc.frame.Y = SPAWN_FRAME * 108;
                    npc.frameCounter = 0;
                } else {
                    AnimationHelper.UpdateAnimation(npc, ANIM_SPAWN, 4);
                }
            } else if (State != AISTATE_JUMP && State != AISTATE_FLY) //walk or charge
            {
                if (npc.velocity.X != 0) {
                    AnimationHelper.UpdateAnimation(npc, ANIM_WALK, 15, (int)Math.Ceiling(Math.Abs(npc.velocity.X)), 108);
                }

                if (npc.velocity.Y != 0 || npc.velocity.X == 0) {
                    if (npc.velocity.Y < 0) {
                        npc.frame.Y = ANIM_JUMP[1] * 108;
                    } else if (npc.velocity.Y > 0) {
                        npc.frame.Y = ANIM_JUMP[2] * 108;
                    } else {
                        AnimationHelper.UpdateAnimation(npc, ANIM_IDLE, 15);
                    }
                }
            } else if (State == AISTATE_FLY) {
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
                npc.frame.Y = ANIM_JUMP[jumpFrame] * 108;
            }
            /*if (State == AISTATE_SPAWN && TimeInState <= 60 + 30) {
                if (TimeInState >= 60) {
                    npc.frame.Y = 1404;
                    npc.frameCounter = 0;
                } else {
                    //npc.frame.Y = 972;
                    npc.frameCounter++;
                    if (npc.frameCounter >= 15) {
                        npc.frameCounter = 0;
                        npc.frame.Y += 108;
                    }
                    if (npc.frame.Y < 972 || npc.frame.Y > 1296) {
                        npc.frame.Y = 972;
                    }
                }
            } else if (State != AISTATE_JUMP && State != AISTATE_FLY) //walk or charge
            {
                npc.frameCounter += (int)Math.Ceiling(Math.Abs(npc.velocity.X));

                if (npc.frameCounter >= 15)
				{
					npc.frameCounter = 0;
					npc.frame.Y += 108;
					if (npc.frame.Y > (108 * 4))
					{
						npc.frameCounter = 0;
						npc.frame.Y = 0;
					}
				}
                if(npc.velocity.Y != 0 || npc.velocity.X == 0)
                {
                    if (npc.velocity.Y < 0)
                    {
                        npc.frame.Y = 648;
                    } else if (npc.velocity.Y > 0)
                    {
                        npc.frame.Y = 756;
                    } else {
                        //npc.frame.Y = 756;
                        if (npc.frame.Y > 108) {
                            npc.frame.Y = 108;
                            npc.frameCounter = 0;
                        }
                        npc.frameCounter++;
                        if (npc.frameCounter >= 15) {
                            if (npc.frame.Y == 108) {
                                npc.frame.Y -= 108;
                            } else {
                                npc.frame.Y = 108;
                            }
                            npc.frameCounter = 0;
                        }
                    }
                }
            }
            else if (State == AISTATE_FLY)
            {
                npc.frameCounter++;
                if (npc.frameCounter >= 2) {
                    npc.frameCounter = 0;
                    if (++flyFrame >= 4) {
                        flyFrame = 0;
                    }
                }
                npc.frame.Y = (flyFrame + 14) * 108;

            }
            else //jump
            {
                if (npc.velocity.Y == 0)
                {
                    npc.frame.Y = 540;
                }else
                {
                    if (npc.velocity.Y < 0)
                    {
                        npc.frame.Y = 648;
                    }else
                    if (npc.velocity.Y > 0)
                    {
                        npc.frame.Y = 756;
                    }
                }
            }*/
            if (State != AISTATE_CHARGE && State != AISTATE_JUMP) {
                if (npc.velocity.X > 0) {
                    npc.spriteDirection = -1;
                } else {
                    npc.spriteDirection = 1;
                }
            }

            for (int m = npc.oldPos.Length - 1; m > 0; m--) {
                npc.oldPos[m] = npc.oldPos[m - 1];
            }
            npc.oldPos[0] = (State == AISTATE_CHARGE /*|| State == AISTATE_FLY*/) ? npc.position : Vector2.Zero;

            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.rotation = 0;

            switch (State) {
                case AISTATE_DEFAULT:
                    if (flyIfOutOfRange(player)) break;

                    float move = player.Center.X - npc.Center.X;
                    move /= Math.Abs(move);
                    move *= 3.0f;
                    Walk(0.17f, move, true, 3, 4, true, null, false);

                    if (TimeInState > 120 && Main.rand.Next(200) == 0) {
                        ChangeState(Main.rand.Next(1, 4));
                        npc.velocity.X = 0;
                    }

                    break;

                case AISTATE_CHARGE:
                    if (TimeInState > 300 || (npc.spriteDirection == -1 && npc.Center.X > player.Center.X + 300) || (npc.spriteDirection == 1 && npc.Center.X < player.Center.X - 300)) {
                        ChangeState(0);
                    } else if (TimeInState > (Main.expertMode ? 10 : 30)) {
                        if (Walk(Main.expertMode ? 0.14f : 0.07f, 10 * -npc.spriteDirection, false, 3, 4, false, null, true)) {
                            /*if (Main.expertMode && Math.Abs(npc.Center.X - AttackParameterF) >= 200) {
                                Projectile.NewProjectile(npc.Center, new Vector2(0f, 0f), mod.ProjectileType("FakeMonarchMushroom"), 0, 0);
                                AttackParameterF = npc.Center.X;
                            }*/
                        } else {
                            ChangeState(0);
                            // TODO: screen shake
                        }
                    } else {
                        if (npc.Center.X < player.Center.X) {
                            npc.spriteDirection = -1;
                        } else {
                            npc.spriteDirection = 1;
                        }
                        AttackParameterF = npc.Center.X;
                    }

                    break;

                case AISTATE_JUMP: {
                      if (npc.velocity.Y == 0) {
                            if (flyIfOutOfRange(player)) break;

                            int newDirection;
                            if (npc.Center.X < player.Center.X) {
                                newDirection = -1;
                            } else {
                                newDirection = 1;
                            }
                            if (newDirection != npc.spriteDirection) {
                                npc.spriteDirection = newDirection;
                                if (AttackParameter2I > 0) longJump = false;
                            }

                            if (AttackParameter2I >= 6) {
                                ChangeState(0);
                                AttackParameterI = 0;
                                AttackParameter2I = 0;
                                longJump = Main.expertMode;
                                break;
                            }

                            if (longJump) {
                                npc.velocity.X = 0;

                                AttackParameterI++;
                                
                                if (AttackParameterI >= (AttackParameter2I == 0 ? 15 : 5)) {
                                    npc.velocity.Y = -8;
                                    npc.velocity.X = -5 * npc.spriteDirection;
                                    npc.noTileCollide = true;
                                    AttackParameterI = 0;
                                    AttackParameter2I++;
                                    if (Main.expertMode) longJump = true;
                                }
                            } else {
                                npc.velocity.X = 0;

                                AttackParameterI++;
                                if (AttackParameterI >= (Main.expertMode ? 10 : 40)) {
                                    float speed = Math.Abs(player.Center.X -  npc.Center.X);
                                    speed = Math.Min(speed / 53, 8);
                                    npc.velocity.Y = -8;
                                    npc.velocity.X = -speed * npc.spriteDirection;
                                    npc.noTileCollide = true;
                                    AttackParameterI = 0;
                                    AttackParameter2I++;
                                    if (Main.expertMode) longJump = true;
                                }
                            }
                        }

                        if (npc.velocity.Y >= 0) npc.noTileCollide = false;

                        break;
                    }
                case AISTATE_SPAWN: {
                        Main.NewText(TimeInState);
                        npc.velocity.X = 0;
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
                        npc.noGravity = true;
                        npc.noTileCollide = true;
                        Vector2 desiredMovement = (FlyTo - npc.Bottom) / 5;
                        Vector2 acceleration = desiredMovement - npc.velocity;
                        acceleration.Normalize();
                        npc.velocity += acceleration;
                        if (npc.velocity.Length() > 15) {
                            npc.velocity.Normalize();
                            npc.velocity *= 15;
                        }
                        npc.rotation = npc.velocity.X / 50;

                        if (Vector2.Distance(FlyTo, npc.Bottom) < 20) ChangeState(0);

                        break;
                    }
            }
            TimeInState++;

            return;

            /*if (npc.collideX && npc.velocity.Y <= 0)
            {
                npc.velocity.Y = -4f;
                internalAI[1] = AISTATE_CHARGE;
            }
            else if (((player.Center.Y - npc.Center.Y) < -150f && (internalAI[1] == AISTATE_WALK || internalAI[1] == AISTATE_CHARGE)) || Collision.SolidCollision(new Vector2(npc.Center.X, npc.position.Y - npc.height/2 + 10), npc.width, npc.height))
            {
                internalAI[1] = AISTATE_FLY;
                npc.ai = new float[4];
                npc.netUpdate = true;
            }
            else if ((player.Center.Y - npc.Center.Y) > 100f && internalAI[1] != AISTATE_FLY) // player is below the npc.
            {
                internalAI[3] = internalAI[1]; //record the action
                internalAI[1] = AISTATE_WALK;
                npc.ai = new float[4];
                npc.netUpdate = true;
            }
            else if(internalAI[1] != AISTATE_WALK)
            {
                internalAI[3] = internalAI[1];
            }
            else
            {
                internalAI[1] = internalAI[3];
            }

            
			if(Main.netMode != 1)
			{
                if (internalAI[1] != AISTATE_FLY)
                {
                    internalAI[0]++;
                }
                if (internalAI[0] >= 180)
                {
                    internalAI[0] = 0;
                    internalAI[1] = Main.rand.Next(3);
                    npc.ai = new float[4];
                    npc.netUpdate = true;
                }
			}
			if(internalAI[1] == AISTATE_WALK) //fighter
			{
                npc.noGravity = false;
                if (Main.netMode != 1)
                {
                    internalAI[2]++;
                }
                if ((player.Center.Y - npc.Center.Y) > 60f) // player is below the npc.
                {
                    npc.noTileCollide = true;
                }
                else
                {
                    npc.noTileCollide = false;
                }

                if (NPC.CountNPCS(ModContent.NPCType<RedMushling>()) < 4)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int Minion = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<RedMushling>(), 0);
                        Main.npc[Minion].netUpdate = true;
                    }
                    internalAI[2] = 0;
                }
                //Walk(0.07f, 3f, 3, 4, true, null, false);
                AAAI.InfernoFighterAI(npc, ref npc.ai, false, false, 0, 0.07f, 3f, 3, 4, 60, true, 10, 60, true, null, false);	
			}else
			if(internalAI[1] == AISTATE_JUMP)//jumper
			{
                npc.noGravity = false;
                npc.noTileCollide = false;
                if(npc.ai[0] < -10) npc.ai[0] = -10; //force rapid jumping
                BaseAI.AISlime(npc, ref npc.ai, true, 30, 6f, -8f, 6f, -10f);
								
			}
            else if (internalAI[1] == AISTATE_FLY)//fly
            {
                npc.noTileCollide = true;
                npc.noGravity = true;
                if((player.Center.Y - npc.Center.Y) > 60f)
                {  
                    if (NPC.CountNPCS(ModContent.NPCType<RedMushling>()) < 6)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            int Minion = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<RedMushling>(), 0);
                            Main.npc[Minion].netUpdate = true;
                        }
                    }
                    MoveToPoint(player.Center);
                    
                }
                else
                {
                    BaseAI.AISpaceOctopus(npc, ref npc.ai, .05f, 8, 250, 0, null);
                }
                
                
                npc.rotation = 0;
                if ((player.Center.Y - npc.Center.Y) > 30f && !Collision.SolidCollision(new Vector2(npc.Center.X, npc.position.Y - npc.height/2 + 10), npc.width, npc.height))
                {
                    npc.rotation = 0;
                    npc.noGravity = false;
                    internalAI[0] = 0;
                    internalAI[1] = Main.rand.Next(3);
                    npc.ai = new float[4];
                    npc.netUpdate = true;
                    npc.noTileCollide = false;
                }
            }
            else //charger
			{
                BaseAI.AICharger(npc, ref npc.ai, 0.07f, 10f, false, 30);				
			}*/
        }

        private bool flyIfOutOfRange(Player player) {
            if (npc.velocity.Y == 0 && player.velocity.Y == 0 && Math.Abs(player.Center.Y - npc.Center.Y) > player.height + npc.height) {
                ChangeState(AISTATE_FLY);
                FlyTo = player.Center;
                return true;
            }
            return false;
        }

        private void ChangeState(int newState) {
            if (State != AISTATE_FLY && newState != AISTATE_FLY) {
                TimeInState = 0;
            }
            if (Main.expertMode) {
                if (State == AISTATE_FLY) {
                    npc.damage = npc.defDamage;
                } else if (newState == AISTATE_FLY) {
                    npc.damage *= 2;
                }
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


