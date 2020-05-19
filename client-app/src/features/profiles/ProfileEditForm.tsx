import React from 'react'
import { IProfile } from '../../app/models/profile';
import { Form as FinalForm, Field } from "react-final-form";

import { Form, Button } from "semantic-ui-react";
import TextInput from '../../app/common/form/TextInput';
import TextAreaInput from '../../app/common/form/TextAreaInput';
import { combineValidators, isRequired } from 'revalidate';

interface IProps {
    updateProfile: (profile: IProfile) => Promise<void>,
    profile: IProfile
}

const validate = combineValidators({
    displayName: isRequired({ message: "The displayName is required." })
});

const ProfileEditForm: React.FC<IProps> = ({ updateProfile, profile }) => {
    return (
        <FinalForm
            onSubmit={updateProfile}
            validate={validate}
            initialValues={profile!}
            render={({ handleSubmit, invalid, pristine, submitting }) => (
                <Form onSubmit={handleSubmit} error>
                    <Field name='displayName' component={TextInput} value={profile!.displayName} />
                    <Field name='bio' component={TextAreaInput} value={profile!.bio} />
                    <Button
                        loading={submitting}
                        floated='right'
                        disabled={invalid || pristine}
                        positive
                        content='Update profile'
                    />
                </Form>
            )}
        />
    )
}

export default ProfileEditForm;
