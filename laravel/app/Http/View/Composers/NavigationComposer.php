<?php

namespace App\Http\View\Composers;

use Illuminate\View\View;
use App\Repositories\CategoryRepository;

class NavigationComposer
{
    /**
     * 实现 UserRepository
     *
     * @var UserRepository
     */
    protected static $category;

    /**
     * 创建一个新的 profile 合成器.
     *
     * @param  UserRepository  $users
     * @return void
     */
    public function __construct(CategoryRepository $category)
    {
        // Dependencies automatically resolved by service container...
        //$this->users = $users;
        self::$category = $category;
    }

    /**
     * 将数据绑定到视图
     *
     * @param  View  $view
     * @return void
     */
    public function compose(View $view)
    {
        $view->with('categories', self::$category->getCates());

        //print($view);
    }
}