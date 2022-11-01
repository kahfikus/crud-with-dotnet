<template>
    <div>
        <div>
            <h1>Registration Form</h1>
            <validation-observer ref="observer" v-slot="{validate,valid}">
                <form method="post" @submit.prevent="insertUser">
                    <label class="col-form-label">User Name</label>
                    <validation-provider name="User Name" rules="required" v-slot="{errors}">
                        <input class="form-control" type="text" v-model="newUser.userName" />
                        <span class="text-danger font-italic">{{errors[0]}}</span>
                    </validation-provider>
                    <br />

                    <label class="col-form-label">Password</label>
                    <validation-provider name="Password" rules="required|min:8" v-slot="{errors}">
                        <input class="form-control" type="password" v-model="newUser.password" />
                        <span class="text-danger font-italic">{{errors[0]}}</span>
                    </validation-provider>
                    <br />

                    <label class="col-form-label">Confirm Password</label>
                    <validation-provider name="Password" rules="required|min:8" v-slot="{errors}">
                        <input class="form-control" type="password" v-model="confirm" />
                        <span class="text-danger font-italic">{{errors[0]}}</span>
                    </validation-provider>
                    <br />

                    <button class="btn btn-primary" type="submit">
                        <fa-icon icon="check"></fa-icon>
                        Submit
                    </button>
                </form>
            </validation-observer>
        </div>
    </div>
</template>

<script lang="ts">
 import Vue from 'vue';
    import Component from 'vue-class-component';
    import Swal from 'sweetalert2';
    import { ValidationObserver } from 'vee-validate';
    import { CartService , InsertUserModel } from '../Services/NSwagService';

@Component({
})
export default class RegistrationUser extends Vue {
    services: CartService = new CartService();

    newUser: InsertUserModel = {
        userId: 0,
        userName: '',
        password: '',
        userRole: 'User'
    }

    isExist: InsertUserModel = {
        userId: 0,
        userName: '',
        password: '',
        userRole: 'User'
    }

    confirm = '';

    async insertUser(): Promise<void> {
            const valid = await (this.$refs.observer as InstanceType<typeof ValidationObserver>).validate();
            if (valid == false) {
                return;
            }

            this.isExist = await this.services.usernameExist(this.newUser.userName);

            if (this.isExist != null) {
                Swal.fire({
                    title: 'Failed!',
                    icon: 'error',
                    text: 'Username already exist!'
                });
            }
            else if (this.newUser.password != this.confirm) {
                Swal.fire({
                    title: 'Failed!',
                    icon: 'error',
                    text: 'Confirmation Password does not match'
                });
            }
            else {
                await this.services.insertUser(this.newUser);

                Swal.fire({
                    title: 'Success!',  
                    icon: 'success',
                    text: 'Your registration is successful!.'
                }).then(function () {
                    window.location.href = '/Auth/Login';
                });

            }
    }
}
</script>
