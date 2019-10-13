<?php

namespace App\Model;

use Illuminate\Database\Eloquent\Model;

class Category extends Model
{
    //
    //var $code = "";
    //var $description = "";
    protected $fillable = ['code', 'description'];


    public function categoryAncestorPaths()
    {
        return $this->hasMany('App\Model\CategoryTreePath', "ancestor");
    }

    public function categoryDescendantPaths()
    {
        return $this->hasMany('App\Model\CategoryTreePath', "descendant");
    }

    public function Posts()
    {
        return $this->hasMany('App\Model\Post');
    }
}
