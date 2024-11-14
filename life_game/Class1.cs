using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace coursework_wpf
{
    public class Const
    { public const int MAX_GRID = 12; }
        

    public class Cell  // клетка
    {
        public BitArray CellStateFlags = new BitArray(5); 

        public override string ToString()
        {
            return $"{CellStateFlags} {GetCellType()}";
        }

        public virtual void activate(int pos, ref Cell[] arr)
        {
            
                int xx = pos % Const.MAX_GRID;
                int yy = (pos - xx) / Const.MAX_GRID;
                int ix = xx - 1, ix2 = xx + 1, iy = yy - 1, iy2 = yy + 1;
                int howmany = 0;
                if (ix < 0) ix = 0;
                if (iy < 0) iy = 0;
                if (ix2 > Const.MAX_GRID - 1) ix2 = Const.MAX_GRID - 1;
                if (iy2 > Const.MAX_GRID - 1) iy2 = Const.MAX_GRID - 1;
                for (int t1 = ix; t1 < ix2 + 1; t1++)
                {
                    for (int t2 = iy; t2 < iy2 + 1; t2++)
                    {
                        if (arr[t1 + t2 * Const.MAX_GRID].GetCellType() == 1) howmany++;
                    }
                }
                if (howmany == 3) arr[pos] = new usual();
            
        }
        public virtual void spawn(int pos, ref Cell[] arr)
        {
            if (arr[pos].CellStateFlags[0]) arr[pos] = new killer();  
            else
            {
                if (arr[pos].CellStateFlags[1]) arr[pos] = new usual();
                else
                {
                    if (arr[pos].CellStateFlags[2]) arr[pos] = new social();
                    else
                    {
                        if (arr[pos].CellStateFlags[3]) arr[pos] = new helther();
                        else
                        {

                        }
                    }
                }
            }
            arr[pos].CellStateFlags = new BitArray(5);
            for (int bi = 0; bi < 5; bi++) CellStateFlags[bi] = false;
        }
        public virtual int GetCellType ()
        { return 5; }

        
    }

    public class killer : Cell // клетка  
    {
        public override void activate(int pos, ref Cell[] arr)
        {
            int xx =pos % Const.MAX_GRID;
            int yy = (pos - xx) / Const.MAX_GRID;
            int ix=xx-2, ix2=xx+2, iy=yy-2, iy2=yy+2;
            bool fl_d=false;
            if (ix < 0) ix = 0;
            if (iy < 0) iy = 0;
            if (ix2 > Const.MAX_GRID - 1) ix2 = Const.MAX_GRID - 1;
            if (iy2 > Const.MAX_GRID - 1) iy2 = Const.MAX_GRID - 1;
            for (int t1 = ix; t1 < ix2 + 1; t1++)
            {
                for (int t2 = iy; t2 < iy2 + 1; t2++)
                {
                    if (arr[t1 + t2 * Const.MAX_GRID].GetCellType() != 5 && arr[t1 + t2 * Const.MAX_GRID].GetCellType() != 0) fl_d = true;
                }
            }
            if (fl_d)
            {
                ix = xx - 1; ix2 = xx + 1; iy = yy - 1; iy2 = yy + 1;
                if (ix < 0) ix = 0;
                if (iy < 0) iy = 0;
                if (ix2 > Const.MAX_GRID - 1) ix2 = Const.MAX_GRID - 1;
                if (iy2 > Const.MAX_GRID - 1) iy2 = Const.MAX_GRID - 1;
                for (int t1 = ix; t1 < ix2 + 1; t1++)
                {
                    for (int t2 = iy; t2 < iy2 + 1; t2++)
                    {
                        arr[t1 + t2 * Const.MAX_GRID].CellStateFlags[0] = true;
                    }
                }
            }
            else
            {
                arr[pos] = new Cell();
            }


            //убийцы – 
            //любая живность кроме убийцы в радиусе двух клеток -> окружает себя убийцами иначе погибает

        }

        public override void spawn(int pos, ref Cell[] arr)
        {
            arr[pos].CellStateFlags = new BitArray(5);
            for (int bi = 0; bi < 5; bi++) CellStateFlags[bi] = false;
        }

        public override int GetCellType()
        { return 0; }
    }
    public class usual : Cell // клетка 
    {
        public override void activate(int pos, ref Cell[] arr)
        {
            int xx = pos % Const.MAX_GRID;
            int yy = (pos - xx) / Const.MAX_GRID;
            int ix = xx - 1, ix2 = xx + 1, iy = yy - 1, iy2 = yy + 1;
            int howmany = 0;
            if (ix < 0) ix = 0;
            if (iy < 0) iy = 0;
            if (ix2 > Const.MAX_GRID - 1) ix2 = Const.MAX_GRID - 1;
            if (iy2 > Const.MAX_GRID - 1) iy2 = Const.MAX_GRID - 1;
            for (int t1 = ix; t1 < ix2 + 1; t1++)
            {
                for (int t2 = iy; t2 < iy2 + 1; t2++)
                {
                    if (arr[t1 + t2 * Const.MAX_GRID].GetCellType() == 1) howmany++;
                }
            }
            if (!(howmany == 2 || howmany == 3)) arr[pos] = new Cell();
            // обычные – 
            //три обычные вокруг пустой -> становится обычной
            //2 - 3 соседа у обычной->живет дальше

        }
        public override void spawn(int pos, ref Cell[] arr)
        {
            arr[pos].CellStateFlags = new BitArray(5);
            for (int bi = 0; bi < 5; bi++) CellStateFlags[bi] = false;
        }
        public override int GetCellType()
        { return 1; }
    }
    public class social : Cell // клетка 
    {
        public override void activate(int pos, ref Cell[] arr)
        {
            int xx = pos % Const.MAX_GRID;
            int yy = (pos - xx) / Const.MAX_GRID;
            int ix = xx - 2, ix2 = xx + 2, iy = yy - 2, iy2 = yy + 2;
            bool fl_d = false;
            if (ix < 0) ix = 0;
            if (iy < 0) iy = 0;
            if (ix2 > Const.MAX_GRID - 1) ix2 = Const.MAX_GRID - 1;
            if (iy2 > Const.MAX_GRID - 1) iy2 = Const.MAX_GRID - 1;
            for (int t1 = ix; t1 < ix2 + 1; t1++)
            {
                for (int t2 = iy; t2 < iy2 + 1; t2++)
                {
                    if (arr[t1 + t2 * Const.MAX_GRID].GetCellType() != 5 && arr[t1 + t2 * Const.MAX_GRID].GetCellType() != 0 && arr[t1 + t2 * Const.MAX_GRID].GetCellType() != 4) fl_d = true;
                }
            }
            ix = xx - 1; ix2 = xx + 1; iy = yy - 1; iy2 = yy + 1;
            int fl_d2 = 0;
            if (ix < 0) ix = 0;
            if (iy < 0) iy = 0;
            if (ix2 > Const.MAX_GRID - 1) ix2 = Const.MAX_GRID - 1;
            if (iy2 > Const.MAX_GRID - 1) iy2 = Const.MAX_GRID - 1;
            for (int t1 = ix; t1 < ix2 + 1; t1++)
            {
                for (int t2 = iy; t2 < iy2 + 1; t2++)
                {
                    if (arr[t1 + t2 * Const.MAX_GRID].GetCellType() == 5 || arr[t1 + t2 * Const.MAX_GRID].GetCellType() == 4 ) fl_d2++;
                }
            }
            if (fl_d && (fl_d2>2))
            {
                ix = xx - 1; ix2 = xx + 1; iy = yy - 1; iy2 = yy + 1;
                if (ix < 0) ix = 0;
                if (iy < 0) iy = 0;
                if (ix2 > Const.MAX_GRID - 1) ix2 = Const.MAX_GRID - 1;
                if (iy2 > Const.MAX_GRID - 1) iy2 = Const.MAX_GRID - 1;
                for (int t1 = ix; t1 < ix2 + 1; t1++)
                {
                    for (int t2 = iy; t2 < iy2 + 1; t2++)
                    {
                        arr[t1 + t2 * Const.MAX_GRID].CellStateFlags[2] = true;
                    }
                }

            }
            else
            {
                arr[pos] = new Cell();
            }
            //           социальные – 
            //любая живность кроме убийцы в радиусе двух клеток и три свободные ячейки -> окружает себя по диагоналям социальными иначе погибает

        }
        public override void spawn(int pos, ref Cell[] arr)
        {
            
            arr[pos].CellStateFlags = new BitArray(5);
            for (int bi = 0; bi < 5; bi++) CellStateFlags[bi] = false;
        }
        public override int GetCellType()
        { return 2; }
    }
    public class helther : Cell // клетка 
    {
        public override void activate(int pos, ref Cell[] arr)
        {
            int xx = pos % Const.MAX_GRID;
            int yy = (pos - xx) / Const.MAX_GRID;
            //          жиживучие – 
            //если рядом есть живучий и одна не занятая клетка -> создает еще одного иначе погибает
            

        }
        public override void spawn(int pos, ref Cell[] arr)
        {
            arr[pos].CellStateFlags = new BitArray(5);
            for (int bi = 0; bi < 5; bi++) CellStateFlags[bi] = false;
        }
        public override int GetCellType()
        { return 3; }
    }
    public class food : Cell // клетка 
    {
        public override void activate(int pos, ref Cell[] arr)
        {    }
        public override void spawn(int pos, ref Cell[] arr)
        {
            if (arr[pos].CellStateFlags[0]) arr[pos] = new killer();  
            else
            {
                if (arr[pos].CellStateFlags[1]) arr[pos] = new usual();
                else
                {
                    if (arr[pos].CellStateFlags[2]) arr[pos] = new social();
                    else
                    {
                        if (arr[pos].CellStateFlags[3]) arr[pos] = new helther();
                        else
                        {

                        }
                    }
                }
            }
            arr[pos].CellStateFlags = new BitArray(5);
            for (int bi = 0; bi < 5; bi++) CellStateFlags[bi] = false;
        }
        public override int GetCellType()
        { return 4; }
    }

    public class Net 
    {

        public Cell[] Cells;

        public Net() 
        {
            Cells = new Cell[Const.MAX_GRID * Const.MAX_GRID];
            for (int t = 0; t < Const.MAX_GRID * Const.MAX_GRID; t++)
                {
                //Console.Write(" ");
                    Cells[t] = new Cell();
                }
        
        }
        
    public override string ToString()
        {
            String x = "";
            for (int t=0;t< Const.MAX_GRID * Const.MAX_GRID; t++)
            {
                x += $"{Cells[t]} ";
            }
            return x;
        }
        

        
    }
}