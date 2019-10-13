<?php

use Illuminate\Database\Seeder;
use App\Model\Category;

class CategoryTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        //
        Category::truncate();

        factory(Category::class, 20)
        ->create()
        ->each(function ($category) {
            $user->posts()->save(factory(App\Post::class)->make());
        });;
    }
}
