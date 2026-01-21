import { ButtonForm, FormFooterRow, FormHeader, InputField, Logo } from "@/components/connexion/FormContainerSignIn";

export function FormContainerResetPassword() {

    return (
        <div className="flex flex-col gap-y-8 px-5 py-8 md:w-112 md:h-auto md:px-8 md:py-10 rounded-12 bg-neutral-0">
            <Logo />
            <FormHeader text="Reset Your Password" supportingText="Enter your new password below. Make sure it’s strong and secure." />
            <form className="flex flex-col gap-y-4">
                <InputField labelInput="New Password *" typeInput="password" />
                <InputField labelInput="Confirm Password *" typeInput="password" />
                <ButtonForm textButton="Reset password" />
            </form>
            <div className="flex flex-col items-center gap-y-3">
                <FormFooterRow textFooterRow="" textFooterRowLink="Back to login" linkFooterRow="/bookmark-manager-app/" />
            </div>
        </div>
    )
}