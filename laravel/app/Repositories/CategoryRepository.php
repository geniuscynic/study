<?php

namespace App\Repositories;

use App\Model\Category;

class CategoryRepository
{
    protected static $category;
    
    public function __construct(Category $category)
	{
	    self::$category = $category;
    }

    public function getCates($where = null)
    {
        return self::$category::take(3)->get();
    }
}