using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AES.SubTab;

namespace AES
{
    class DRijndael
    {
        private static int Nb, Nk, Nr;
        private static byte[,] w;

        public static byte[] decrypt(byte[] input, byte[] key)
        {
            int i;
            byte[] tmp = new byte[input.Count()];
            byte[] bloc = new byte[16];


            Nb = 4;
            Nk = key.Count() / 4;
            Nr = Nk + 6;
            w = generateSubkeys(key);


            for (i = 0; i < input.Count(); i++) {
                if (i > 0 && i % 16 == 0)
                {
                    bloc = decryptBloc(bloc);
                    Array.Copy(bloc, 0, tmp, i - 16, bloc.Count());
                }
                if (i < input.Count())
				bloc[i % 16] = input[i];
		    }

            bloc = decryptBloc(bloc);
            Array.Copy(bloc, 0, tmp, i - 16, bloc.Count());
            
		    tmp = deletePadding(tmp);

		    return tmp;
	    }

        private static byte[,] generateSubkeys(byte[] key)
        {
            byte[][] tmp = new byte[Nb * (Nr + 1)][];//[4];
            for (int j = 0; j < tmp.Count(); j++)
            {
                tmp[j] = new byte[4];
            }

            int i = 0;
            while (i < Nk)
            {
                tmp[i][0] = key[i * 4];
                tmp[i][1] = key[i * 4 + 1];
                tmp[i][2] = key[i * 4 + 2];
                tmp[i][3] = key[i * 4 + 3];
                i++;
            }
            i = Nk;
            while (i < Nb * (Nr + 1))
            {
                byte[] temp = new byte[4];
                for (int k = 0; k < 4; k++)
                    temp[k] = tmp[i - 1][k];
                if (i % Nk == 0)
                {
                    temp = SubWord(rotateWord(temp));
                    temp[0] = (byte)(temp[0] ^ (Rcon[i / Nk] & 0xff));
                }
                else if (Nk > 6 && i % Nk == 4)
                {
                    temp = SubWord(temp);
                }
                tmp[i] = xor_func(tmp[i - Nk], temp);
                i++;
            }

            byte[,] res = new byte[Nb * (Nr + 1), 4];
            for (int m = 0; m < tmp.Count(); m++)
            {
                for (int n = 0; n < tmp[0].Count(); n++)
                {
                    res[m, n] = tmp[m][n];
                }
            }

            return res;
        }

        private static byte[] SubWord(byte[] input)
        {
            byte[] tmp = new byte[input.Count()];

            for (int i = 0; i < tmp.Count(); i++)
            {
                tmp[i] = (byte)(sbox[input[i] & 0x000000ff] & 0xff);
            }

            return tmp;
        }

        private static byte[] rotateWord(byte[] input)
        {
            byte[] tmp = new byte[input.Count()];
            tmp[0] = input[1];
            tmp[1] = input[2];
            tmp[2] = input[3];
            tmp[3] = input[0];

            return tmp;
        }

        private static byte[] xor_func(byte[] a, byte[] b)
        {
            byte[] tmp = new byte[a.Count()];
            for (int i = 0; i < a.Count(); i++)
            {
                tmp[i] = (byte)(a[i] ^ b[i]);
            }

            return tmp;
        }

        public static byte[] decryptBloc(byte[] input)
        {
            byte[] tmp = new byte[input.Count()];

            byte[,] state = new byte[4,Nb];

            for (int i = 0; i < input.Count(); i++)
			state[i / 4,i % 4] = input[i%4*4+i/4];

		    state = AddRoundKey(state, w, Nr);
		    for (int round = Nr - 1; round >=1; round--) {
			    state = InvSubBytes(state);
                state = InvShiftRows(state);
                state = AddRoundKey(state, w, round);
                state = InvMixColumns(state);
            }

            state = InvSubBytes(state);
            state = InvShiftRows(state);
            state = AddRoundKey(state, w, 0);

		    for (int i = 0; i<tmp.Count(); i++)
            {
                tmp[i % 4 * 4 + i / 4] = state[i / 4,i % 4];
            }

		    return tmp;
	    }

        private static byte[,] AddRoundKey(byte[,] state, byte[,] w, int round)
        {
            int count = (int)Math.Sqrt(state.Length);
            byte[,] tmp = new byte[count, count];

            for (int c = 0; c < Nb; c++)
            {
                for (int l = 0; l < 4; l++)
                    tmp[l, c] = (byte)(state[l, c] ^ w[round * Nb + c, l]);
            }

            return tmp;
        }

        private static byte[,] InvSubBytes(byte[,] state)
        {
            for (int row = 0; row < 4; row++)
                for (int col = 0; col < Nb; col++)
                    state[row,col] = (byte)(inv_sbox[(state[row,col] & 0x000000ff)] & 0xff);

            return state;
        }

        private static byte[,] InvShiftRows(byte[,] state)
        {
            byte[] t = new byte[4];
            for (int r = 1; r < 4; r++)
            {
                for (int c = 0; c < Nb; c++)
                    t[(c + r) % Nb] = state[r,c];
                for (int c = 0; c < Nb; c++)
                    state[r,c] = t[c];
            }
            return state;
        }

        private static byte[,] InvMixColumns(byte[,] s)
        {
            int[] sp = new int[4];
            byte b02 = (byte)0x0e, b03 = (byte)0x0b, b04 = (byte)0x0d, b05 = (byte)0x09;
            for (int c = 0; c < 4; c++)
            {
                sp[0] = FFMul(b02, s[0,c]) ^ FFMul(b03, s[1,c]) ^ FFMul(b04, s[2,c]) ^ FFMul(b05, s[3,c]);
                sp[1] = FFMul(b05, s[0,c]) ^ FFMul(b02, s[1,c]) ^ FFMul(b03, s[2,c]) ^ FFMul(b04, s[3,c]);
                sp[2] = FFMul(b04, s[0,c]) ^ FFMul(b05, s[1,c]) ^ FFMul(b02, s[2,c]) ^ FFMul(b03, s[3,c]);
                sp[3] = FFMul(b03, s[0,c]) ^ FFMul(b04, s[1,c]) ^ FFMul(b05, s[2,c]) ^ FFMul(b02, s[3,c]);
                for (int i = 0; i < 4; i++) s[i,c] = (byte)(sp[i]);
            }

            return s;
        }

        public static byte FFMul(byte a, byte b)
        {
            byte aa = a, bb = b, r = 0, t;
            while (aa != 0)
            {
                if ((aa & 1) != 0)
                {
                    r = (byte)(r ^ bb);
                }
                t = (byte)(bb & 0x80);
                bb = (byte)(bb << 1);
                if (t != 0)
                {
                    bb = (byte)(bb ^ 0x1b);
                }
                aa = (byte)((aa & 0xff) >> 1);
            }

            return r;
        }

        private static byte[] deletePadding(byte[] input)
        {
            int count = 0;

            int i = input.Count() - 1;
            while (input[i] == 0)
            {
                count++;
                i--;
            }

            byte[] tmp = new byte[input.Count() - count - 1];
            Array.Copy(input, 0, tmp, 0, tmp.Count());
            return tmp;
        }

    }
}
