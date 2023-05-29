using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Scripts.Chips
{
    public enum ChipState
    {
        Idle = 0,       //Анимация простоя
        Appear,         //Анимация появления при создании
        BonusAppear,    //Появление бонуса при сборе комбинации
        Destroy,        //Запуск анимации уничтожения
        Destroyed,      //Окончательное уничтожение объекта
        Activate,       //Активация бонуса
        Explosion,      //Взрыв после активации
        Tip,             //Выделение подсказкой
        Move
    }
}
