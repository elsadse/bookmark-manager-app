import { ButtonForm, FormFooterRow, FormHeader, InputField, Logo } from "@/components/connexion/FormContainerSignIn";

export function FormContainerForgotPassword() {

    return (
        <div className="flex flex-col gap-y-8 px-5 py-8 md:w-112 md:h-auto md:px-8 md:py-10 rounded-12 bg-neutral-0">
            <Logo />
            <FormHeader text="Forgot your password?" supportingText="Enter your email address below and we’ll send you a link to reset your password." />
            <form className="flex flex-col gap-y-4">
                <InputField labelInput="Email *" typeInput="email" />
                <ButtonForm textButton="Send reset link" />
            </form>
            <div className="flex flex-col items-center gap-y-3">
                <FormFooterRow textFooterRow="" textFooterRowLink="Back to login" linkFooterRow="/" />
            </div>
        </div>
    )
}