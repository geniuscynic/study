<?php

namespace App\Model;

use Illuminate\Database\Eloquent\Model;

class Post extends Model
{
    //
    //protected $fillable = ['title', 'author', 'souce_link'];

    public function category()
    {
        return $this->belongsTo('App\Model\Category');
    }

    public function tag()
    {
        return $this->belongsToMany('App\Model\Tag');
    }
}
